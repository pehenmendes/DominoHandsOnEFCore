using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Repositories;

namespace DominoPontaDeQuina.Services.Services;

public class ParticipacaoPartidaService(IParticipacaoPartidaRepository repository) : IParticipacaoPartidaService
{
    public Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancellationToken = default) => repository.ListarPorPartidaAsync(partidaId, cancellationToken);
    public Task<List<ParticipacaoPartida>> ListarVencedoresAsync(CancellationToken cancellationToken = default) => repository.ListarVencedoresAsync(cancellationToken);
}
