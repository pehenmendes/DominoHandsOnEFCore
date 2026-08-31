using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Services;

public interface IPartidaService
{
    Task<Partida> CriarAsync(CancellationToken cancellationToken = default);
    Task<Partida> AdicionarJogadorAsync(Guid partidaId, Guid jogadorId, int posicao, CancellationToken cancellationToken = default);
    Task<Partida> IniciarAsync(Guid partidaId, CancellationToken cancellationToken = default);
    Task<Partida> FinalizarAsync(Guid partidaId, CancellationToken cancellationToken = default);
    Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancellationToken = default);
    Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
