using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Services;

public interface IParticipacaoPartidaService
{
    Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancellationToken = default);
    Task<List<ParticipacaoPartida>> ListarVencedoresAsync(CancellationToken cancellationToken = default);
}
