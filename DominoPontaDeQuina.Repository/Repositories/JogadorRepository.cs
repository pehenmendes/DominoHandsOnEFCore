using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class JogadorRepository(DominoDbContext context) : IJogadorRepository
{
    public Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Jogadores.Include(j => j.Usuario).FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    public Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        context.Jogadores.AsNoTracking().Where(j => j.UsuarioId == usuarioId).OrderBy(j => j.NomeExibicao).ToListAsync(cancellationToken);

    public Task<List<Jogador>> BuscarPorNomeAsync(string nome, CancellationToken cancellationToken = default) =>
        context.Jogadores.AsNoTracking().Where(j => j.NomeExibicao.Contains(nome)).OrderBy(j => j.NomeExibicao).ToListAsync(cancellationToken);

    public Task<List<Jogador>> ListarAsync(CancellationToken cancellationToken = default) =>
        context.Jogadores.AsNoTracking().OrderBy(j => j.NomeExibicao).ToListAsync(cancellationToken);

    public async Task AdicionarAsync(Jogador jogador, CancellationToken cancellationToken = default)
    {
        await context.Jogadores.AddAsync(jogador, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Jogador jogador, CancellationToken cancellationToken = default)
    {
        context.Jogadores.Update(jogador);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var jogador = await context.Jogadores.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        if (jogador is null) return;
        context.Jogadores.Remove(jogador);
        await context.SaveChangesAsync(cancellationToken);
    }
}
