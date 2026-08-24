using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Repositories;

public interface IPartidaRepository
{
    Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Partida>> ListarPorStatusAsync(StatusJogo status, CancellationToken cancellationToken = default);
    Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancellationToken = default);
    Task<List<Partida>> ListarAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Partida partida, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Partida partida, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
