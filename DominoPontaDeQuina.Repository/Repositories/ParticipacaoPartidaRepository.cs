using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class ParticipacaoPartidaRepository(DominoDbContext context) : IParticipacaoPartidaRepository
{
    public Task<ParticipacaoPartida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.ParticipacoesPartida.Include(p => p.Partida).Include(p => p.Jogador)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<ParticipacaoPartida?> ObterPorPartidaEJogadorAsync(Guid partidaId, Guid jogadorId, CancellationToken cancellationToken = default) =>
        context.ParticipacoesPartida.FirstOrDefaultAsync(p => p.PartidaId == partidaId && p.JogadorId == jogadorId, cancellationToken);

    public Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancellationToken = default) =>
        context.ParticipacoesPartida.AsNoTracking().Where(p => p.PartidaId == partidaId)
            .Include(p => p.Jogador).OrderBy(p => p.Posicao).ToListAsync(cancellationToken);

    public Task<List<ParticipacaoPartida>> ListarVencedoresAsync(CancellationToken cancellationToken = default) =>
        context.ParticipacoesPartida.AsNoTracking().Where(p => p.Vencedor)
            .Include(p => p.Jogador).Include(p => p.Partida)
            .OrderByDescending(p => p.Pontuacao).ToListAsync(cancellationToken);

    public async Task AdicionarAsync(ParticipacaoPartida participacao, CancellationToken cancellationToken = default)
    {
        await context.ParticipacoesPartida.AddAsync(participacao, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(ParticipacaoPartida participacao, CancellationToken cancellationToken = default)
    {
        context.ParticipacoesPartida.Update(participacao);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var participacao = await context.ParticipacoesPartida.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (participacao is null) return;
        context.ParticipacoesPartida.Remove(participacao);
        await context.SaveChangesAsync(cancellationToken);
    }
}
