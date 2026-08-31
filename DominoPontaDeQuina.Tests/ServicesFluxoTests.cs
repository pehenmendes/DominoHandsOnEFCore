using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using DominoPontaDeQuina.Services.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Tests;

public class ServicesFluxoTests
{
    private static (SqliteConnection Connection, DominoDbContext Context, IUsuarioService Usuarios, IJogadorService Jogadores, IPartidaService Partidas) CriarSut()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<DominoDbContext>().UseSqlite(connection).Options;
        var context = new DominoDbContext(options);
        context.Database.EnsureCreated();

        IUsuarioRepository usuarioRepository = new UsuarioRepository(context);
        IJogadorRepository jogadorRepository = new JogadorRepository(context);
        IPartidaRepository partidaRepository = new PartidaRepository(context);
        IParticipacaoPartidaRepository participacaoRepository = new ParticipacaoPartidaRepository(context);

        return (connection, context,
            new UsuarioService(usuarioRepository),
            new JogadorService(jogadorRepository, usuarioRepository),
            new PartidaService(partidaRepository, jogadorRepository, participacaoRepository));
    }

    [Fact]
    public async Task DeveCriarUsuarioEJogadorPorService()
    {
        var sut = CriarSut();
        try
        {
            var usuario = await sut.Usuarios.CriarAsync("Pedro", "pedro@teste.com", "hash");
            var jogador = await sut.Jogadores.CriarAsync(usuario.Id, "Pedro jogador");

            var encontrados = await sut.Jogadores.ListarPorUsuarioAsync(usuario.Id);

            Assert.Equal(jogador.Id, encontrados.Single().Id);
        }
        finally { await sut.Context.DisposeAsync(); await sut.Connection.DisposeAsync(); }
    }

    [Fact]
    public async Task DeveOrquestrarCriacaoAdicaoEInicioDaPartida()
    {
        var sut = CriarSut();
        try
        {
            var usuario = await sut.Usuarios.CriarAsync("Pedro", "pedro2@teste.com", "hash");
            var j1 = await sut.Jogadores.CriarAsync(usuario.Id, "J1");
            var j2 = await sut.Jogadores.CriarAsync(usuario.Id, "J2");
            var partida = await sut.Partidas.CriarAsync();

            await sut.Partidas.AdicionarJogadorAsync(partida.Id, j1.Id, 1);
            await sut.Partidas.AdicionarJogadorAsync(partida.Id, j2.Id, 2);
            var iniciada = await sut.Partidas.IniciarAsync(partida.Id);

            Assert.Equal(StatusJogo.EmAndamento, iniciada.Status);
            Assert.Equal(2, iniciada.Participacoes.Count);
        }
        finally { await sut.Context.DisposeAsync(); await sut.Connection.DisposeAsync(); }
    }

    [Fact]
    public async Task DeveConsultarPartidasEmAndamentoAtravesDoRepository()
    {
        var sut = CriarSut();
        try
        {
            var usuario = await sut.Usuarios.CriarAsync("Pedro", "pedro3@teste.com", "hash");
            var j1 = await sut.Jogadores.CriarAsync(usuario.Id, "J1");
            var j2 = await sut.Jogadores.CriarAsync(usuario.Id, "J2");
            var partida = await sut.Partidas.CriarAsync();
            await sut.Partidas.AdicionarJogadorAsync(partida.Id, j1.Id, 1);
            await sut.Partidas.AdicionarJogadorAsync(partida.Id, j2.Id, 2);
            await sut.Partidas.IniciarAsync(partida.Id);

            var resultado = await sut.Partidas.ListarEmAndamentoAsync();

            Assert.Contains(resultado, p => p.Id == partida.Id);
        }
        finally { await sut.Context.DisposeAsync(); await sut.Connection.DisposeAsync(); }
    }
}
