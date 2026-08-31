using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IRodada"/>
public class Rodada() : IRodada
{
    Stack<Jogada> Jogadas { get; } = [];
    Queue<MaoJogador> _jogadores = [];

    public Tabuleiro Tabuleiro { get; } = new();
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();
    public MaoJogador JogadorAtual => _jogadores.Peek();
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada rodadaAnterior = null)
    {
        ArgumentNullException.ThrowIfNull(jogadores);
        if (jogadores.Count == 0)
            throw new ArgumentException("A rodada precisa de pelo menos um jogador.", nameof(jogadores));

        Tabuleiro.Limpar();
        Jogadas.Clear();
        TipoFinalizacao = null;

        var maosJogadores = DistribuirPecas(jogadores);
        var primeiroJogador = GetPrimeiroJogador(maosJogadores, rodadaAnterior);
        OrganizaJogadores(maosJogadores, primeiroJogador);
        Status = StatusRodada.EmAndamento;
    }

    public void RegistrarJogada(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);
        if (Status is not StatusRodada.EmAndamento)
            throw new InvalidOperationException("Não é possível registrar jogadas em uma rodada que não está em andamento.");

        if (!jogada.EhPassarVez())
            Tabuleiro.Colar(jogada.Peca.Value, jogada.Lado!.Value);

        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();

        // Passar a vez também precisa avançar o turno.
        var atual = _jogadores.Dequeue();
        _jogadores.Enqueue(atual);
    }

    public bool VerificarBatida()
    {
        if (Status is not StatusRodada.EmAndamento)
            return false;

        var vencedor = _jogadores.FirstOrDefault(mao => mao.EstaSemPecas());
        if (vencedor is null)
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
        Status = StatusRodada.Finalizada;
        return true;
    }

    public bool VerificarTabuleiroTravado()
    {
        if (Status is not StatusRodada.EmAndamento)
            return false;

        if (!Tabuleiro.EstaTravado(_jogadores))
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
        Status = StatusRodada.Finalizada;
        return true;
    }

    public Jogador? GetVencedor()
    {
        if (TipoFinalizacao is TipoFinalizacaoRodada.JogadorBateu)
            return _jogadores.FirstOrDefault(mao => mao.EstaSemPecas())?.Jogador;

        if (TipoFinalizacao is TipoFinalizacaoRodada.TabuleiroTravado)
            return _jogadores
                .OrderBy(mao => mao.SomarPecasNaMao())
                .Select(mao => mao.Jogador)
                .FirstOrDefault();

        return null;
    }

    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        const int pecasPorJogador = 7;
        const int totalPecas = 28;

        if (jogadores.Count * pecasPorJogador > totalPecas)
            throw new ArgumentException("Não há peças suficientes para distribuir 7 peças a cada jogador.", nameof(jogadores));

        var baralho = (from a in Enumerable.Range(0, 7)
                       from b in Enumerable.Range(a, 7)
                       select new Peca(a, b)).ToList();

        // Mantém a distribuição aleatória, mas garante uma distribuição sem reposição.
        var rng = Random.Shared;
        for (var i = baralho.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (baralho[i], baralho[j]) = (baralho[j], baralho[i]);
        }

        var maos = jogadores.Select(j => new MaoJogador(j)).ToList();
        var indice = 0;
        foreach (var mao in maos)
        {
            for (var i = 0; i < pecasPorJogador; i++)
                mao.AdicionarPeca(baralho[indice++]);
        }

        return maos;
    }

    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
            return rodadaAnterior.GetVencedor() ?? jogadores.First().Jogador;

        return jogadores.FirstOrDefault(mao => mao.PossuiSena())?.Jogador
            ?? jogadores.OrderByDescending(mao => mao.SomarPecasNaMao()).First().Jogador;
    }

    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        var indice = jogadores.FindIndex(mao => ReferenceEquals(mao.Jogador, primeiroJogador));
        if (indice < 0)
            throw new InvalidOperationException("O primeiro jogador não pertence à rodada.");

        _jogadores = new Queue<MaoJogador>(
            jogadores.Skip(indice).Concat(jogadores.Take(indice)));
    }

    private void CalcularPontuacao()
    {
        if (Jogadas.Count == 0)
            return;

        var jogada = Jogadas.Peek();
        if (jogada.EhPassarVez() || jogada.Jogador.Time is null)
            return;

        var pontos = Tabuleiro.SomarPontasExternas();
        if (pontos > 0 && pontos % 5 == 0)
            jogada.Jogador.Time.SomarPontos(pontos);
    }
}
