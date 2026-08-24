using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Usuario>> ListarAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
