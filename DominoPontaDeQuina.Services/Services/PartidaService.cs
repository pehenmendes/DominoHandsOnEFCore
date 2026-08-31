using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Repositories;

namespace DominoPontaDeQuina.Services.Services;

public class PartidaService(IPartidaRepository partidaRepository, IJogadorRepository jogadorRepository, IParticipacaoPartidaRepository participacaoRepository) : IPartidaService
{
    public async Task<Partida> CriarAsync(CancellationToken cancellationToken = default)
    {
        var partida = new Partida();
        await partidaRepository.AdicionarAsync(partida, cancellationToken);
        return partida;
    }

    public async Task<Partida> AdicionarJogadorAsync(Guid partidaId, Guid jogadorId, int posicao, CancellationToken cancellationToken = default)
    {
        var partida = await partidaRepository.ObterPorIdAsync(partidaId, cancellationToken)
            ?? throw new KeyNotFoundException("Partida não encontrada.");
        if (partida.Status != StatusJogo.Aguardando) throw new InvalidOperationException("Só é possível adicionar jogadores a uma partida aguardando.");
        var jogador = await jogadorRepository.ObterPorIdAsync(jogadorId, cancellationToken)
            ?? throw new KeyNotFoundException("Jogador não encontrado.");
        if (await participacaoRepository.ObterPorPartidaEJogadorAsync(partidaId, jogadorId, cancellationToken) is not null)
            throw new InvalidOperationException("O jogador já participa desta partida.");

        await participacaoRepository.AdicionarAsync(new ParticipacaoPartida { PartidaId = partida.Id, Partida = partida, JogadorId = jogador.Id, Jogador = jogador, Posicao = posicao }, cancellationToken);
        return (await partidaRepository.ObterPorIdAsync(partidaId, cancellationToken))!;
    }

    public async Task<Partida> IniciarAsync(Guid partidaId, CancellationToken cancellationToken = default)
    {
        var partida = await partidaRepository.ObterPorIdAsync(partidaId, cancellationToken)
            ?? throw new KeyNotFoundException("Partida não encontrada.");
        if (partida.Status != StatusJogo.Aguardando) throw new InvalidOperationException("A partida não está aguardando início.");
        if (partida.Participacoes.Count < 2) throw new InvalidOperationException("A partida precisa de pelo menos dois jogadores.");
        partida.Status = StatusJogo.EmAndamento;
        partida.IniciadoEm = DateTime.UtcNow;
        await partidaRepository.AtualizarAsync(partida, cancellationToken);
        return partida;
    }

    public async Task<Partida> FinalizarAsync(Guid partidaId, CancellationToken cancellationToken = default)
    {
        var partida = await partidaRepository.ObterPorIdAsync(partidaId, cancellationToken)
            ?? throw new KeyNotFoundException("Partida não encontrada.");
        if (partida.Status != StatusJogo.EmAndamento) throw new InvalidOperationException("Apenas partidas em andamento podem ser finalizadas.");
        partida.Status = StatusJogo.Finalizado;
        partida.FinalizadoEm = DateTime.UtcNow;
        await partidaRepository.AtualizarAsync(partida, cancellationToken);
        return partida;
    }

    public Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancellationToken = default) => partidaRepository.ListarEmAndamentoAsync(cancellationToken);
    public Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) => partidaRepository.ObterPorIdAsync(id, cancellationToken);
}
