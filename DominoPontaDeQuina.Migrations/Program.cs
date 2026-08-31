using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using DominoPontaDeQuina.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<DominoDbContext>(options =>
    options.UseSqlite("Data Source=domino.db", sqlite =>
        sqlite.MigrationsAssembly("DominoPontaDeQuina.Migrations")));

services.AddScoped<IUsuarioRepository, UsuarioRepository>();
services.AddScoped<IJogadorRepository, JogadorRepository>();
services.AddScoped<IPartidaRepository, PartidaRepository>();
services.AddScoped<IParticipacaoPartidaRepository, ParticipacaoPartidaRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddScoped<IUsuarioService, UsuarioService>();
services.AddScoped<IJogadorService, JogadorService>();
services.AddScoped<IPartidaService, PartidaService>();
services.AddScoped<IParticipacaoPartidaService, ParticipacaoPartidaService>();
services.AddScoped<ConsoleApp>();

using var provider = services.BuildServiceProvider();
await provider.GetRequiredService<ConsoleApp>().ExecutarAsync();

public sealed class ConsoleApp(
    DominoDbContext db,
    IUsuarioService usuarioService,
    IJogadorService jogadorService,
    IPartidaService partidaService)
{
    public async Task ExecutarAsync()
    {
        await db.Database.MigrateAsync();

        var email = $"teste.{Guid.NewGuid():N}@domino.local";
        var usuario = await usuarioService.CriarAsync("Usuário de Teste", email, "hash-demo");
        var jogador1 = await jogadorService.CriarAsync(usuario.Id, "Jogador 1");
        var jogador2 = await jogadorService.CriarAsync(usuario.Id, "Jogador 2");

        var partida = await partidaService.CriarAsync();
        await partidaService.AdicionarJogadorAsync(partida.Id, jogador1.Id, 1);
        await partidaService.AdicionarJogadorAsync(partida.Id, jogador2.Id, 2);
        await partidaService.IniciarAsync(partida.Id);

        var partidas = await partidaService.ListarEmAndamentoAsync();
        Console.WriteLine($"Fluxo executado com DI. Partidas em andamento: {partidas.Count}");
    }
}
