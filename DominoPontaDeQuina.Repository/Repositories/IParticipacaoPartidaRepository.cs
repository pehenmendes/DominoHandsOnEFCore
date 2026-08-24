using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Repositories;

public interface IParticipacaoPartidaRepository
{
    Task<ParticipacaoPartida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ParticipacaoPartida?> ObterPorPartidaEJogadorAsync(Guid partidaId, Guid jogadorId, CancellationToken cancellationToken = default);
    Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancellationToken = default);
    Task<List<ParticipacaoPartida>> ListarVencedoresAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(ParticipacaoPartida participacao, CancellationToken cancellationToken = default);
    Task AtualizarAsync(ParticipacaoPartida participacao, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
