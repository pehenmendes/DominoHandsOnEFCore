using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DominoDbContext()
    {
    }

    public DominoDbContext(DbContextOptions<DominoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<ParticipacaoPartida> ParticipacoesPartida => Set<ParticipacaoPartida>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=domino.db", sqliteOptions =>
                sqliteOptions.MigrationsAssembly("DominoPontaDeQuina.Migrations"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacionamento Usuario (1) -> Jogadores (N)
        modelBuilder.Entity<Jogador>()
            .HasOne(j => j.Usuario)
            .WithMany(u => u.Jogadores)
            .HasForeignKey(j => j.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Partida (1) -> Participacoes (N)
        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(p => p.Partida)
            .WithMany(partida => partida.Participacoes)
            .HasForeignKey(p => p.PartidaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Jogador (1) -> Participacoes (N)
        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(p => p.Jogador)
            .WithMany(j => j.Participacoes)
            .HasForeignKey(p => p.JogadorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Um jogador não pode aparecer duas vezes na mesma partida.
        modelBuilder.Entity<ParticipacaoPartida>()
            .HasIndex(p => new { p.PartidaId, p.JogadorId })
            .IsUnique();

        // O e-mail identifica unicamente um usuário.
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
