using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Services;

public interface IUsuarioService
{
    Task<Usuario> CriarAsync(string nome, string email, string hashSenha, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Usuario>> ListarAsync(CancellationToken cancellationToken = default);
}
