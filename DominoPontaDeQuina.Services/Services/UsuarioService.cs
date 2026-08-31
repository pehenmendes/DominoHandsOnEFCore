using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Repositories;

namespace DominoPontaDeQuina.Services.Services;

public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
{
    public async Task<Usuario> CriarAsync(string nome, string email, string hashSenha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("E-mail é obrigatório.", nameof(email));
        if (await repository.ObterPorEmailAsync(email, cancellationToken) is not null)
            throw new InvalidOperationException("Já existe um usuário com este e-mail.");

        var usuario = new Usuario { Nome = nome.Trim(), Email = email.Trim(), HashSenha = hashSenha };
        await repository.AdicionarAsync(usuario, cancellationToken);
        return usuario;
    }

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default) =>
        repository.ObterPorEmailAsync(email, cancellationToken);

    public Task<List<Usuario>> ListarAsync(CancellationToken cancellationToken = default) =>
        repository.ListarAsync(cancellationToken);
}
