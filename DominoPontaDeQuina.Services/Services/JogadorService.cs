using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Repositories;

namespace DominoPontaDeQuina.Services.Services;

public class JogadorService(IJogadorRepository jogadorRepository, IUsuarioRepository usuarioRepository) : IJogadorService
{
    public async Task<Jogador> CriarAsync(Guid usuarioId, string nomeExibicao, CancellationToken cancellationToken = default)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(usuarioId, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");
        if (string.IsNullOrWhiteSpace(nomeExibicao)) throw new ArgumentException("Nome de exibição é obrigatório.", nameof(nomeExibicao));

        var jogador = new Jogador { UsuarioId = usuario.Id, Usuario = usuario, NomeExibicao = nomeExibicao.Trim() };
        await jogadorRepository.AdicionarAsync(jogador, cancellationToken);
        return jogador;
    }

    public Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        jogadorRepository.ObterPorIdAsync(id, cancellationToken);

    public Task<List<Jogador>> BuscarAsync(string nome, CancellationToken cancellationToken = default) =>
        jogadorRepository.BuscarPorNomeAsync(nome, cancellationToken);

    public Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        jogadorRepository.ListarPorUsuarioAsync(usuarioId, cancellationToken);
}
