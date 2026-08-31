using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    List<Peca> _pecas = [];

    internal IReadOnlyList<Peca> Pecas => _pecas;

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        var indice = tabuleiro.EstaVazio
            ? 0
            : _pecas.FindIndex(peca =>
                tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda) ||
                tabuleiro.PodeColar(peca, LadoTabuleiro.Direita));

        if (indice < 0 || _pecas.Count == 0)
            return new Jogada(Jogador);

        var pecaEscolhida = _pecas[indice];
        var lado = tabuleiro.EstaVazio
            ? LadoTabuleiro.Direita
            : tabuleiro.PodeColar(pecaEscolhida, LadoTabuleiro.Direita)
                ? LadoTabuleiro.Direita
                : LadoTabuleiro.Esquerda;

        _pecas.RemoveAt(indice);
        return new Jogada(Jogador, pecaEscolhida, lado == LadoTabuleiro.Esquerda ? tabuleiro.PontaEsquerda : tabuleiro.PontaDireita, lado);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);

        if (jogada.Peca is null || _pecas.Contains(jogada.Peca.Value))
            return;

        _pecas.Add(jogada.Peca.Value);
    }
}