using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class PartidaRepository(DominoDbContext context) : IPartidaRepository
{
    public Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Partidas.Include(p => p.Participacoes).ThenInclude(p => p.Jogador)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<List<Partida>> ListarPorStatusAsync(StatusJogo status, CancellationToken cancellationToken = default) =>
        context.Partidas.AsNoTracking().Where(p => p.Status == status).OrderByDescending(p => p.IniciadoEm).ToListAsync(cancellationToken);

    public Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancellationToken = default) =>
        context.Partidas.AsNoTracking().Where(p => p.Status == StatusJogo.EmAndamento).OrderByDescending(p => p.IniciadoEm).ToListAsync(cancellationToken);

    public Task<List<Partida>> ListarAsync(CancellationToken cancellationToken = default) =>
        context.Partidas.AsNoTracking().OrderByDescending(p => p.IniciadoEm).ToListAsync(cancellationToken);

    public async Task AdicionarAsync(Partida partida, CancellationToken cancellationToken = default)
    {
        await context.Partidas.AddAsync(partida, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Partida partida, CancellationToken cancellationToken = default)
    {
        context.Partidas.Update(partida);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var partida = await context.Partidas.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (partida is null) return;
        context.Partidas.Remove(partida);
        await context.SaveChangesAsync(cancellationToken);
    }
}
