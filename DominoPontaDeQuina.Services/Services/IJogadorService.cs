using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Services;

public interface IJogadorService
{
    Task<Jogador> CriarAsync(Guid usuarioId, string nomeExibicao, CancellationToken cancellationToken = default);
    Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Jogador>> BuscarAsync(string nome, CancellationToken cancellationToken = default);
    Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
}
