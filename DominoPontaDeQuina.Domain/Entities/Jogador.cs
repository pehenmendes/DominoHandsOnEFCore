using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoPontaDeQuina.Domain.Entities;

public class Jogador
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string NomeExibicao { get; set; } = string.Empty;

    [Required]
    public Guid UsuarioId { get; set; }

    [Required]
    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;

    public ICollection<ParticipacaoJogo> Participacoes { get; set; } = new List<ParticipacaoJogo>();
}
