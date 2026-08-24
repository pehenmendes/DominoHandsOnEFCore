using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Repositories;

public interface IJogadorRepository
{
    Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<List<Jogador>> BuscarPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<List<Jogador>> ListarAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Jogador jogador, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Jogador jogador, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
