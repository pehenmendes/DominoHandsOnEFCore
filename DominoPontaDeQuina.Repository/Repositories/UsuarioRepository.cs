using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class UsuarioRepository(DominoDbContext context) : IUsuarioRepository
{
    public Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default) =>
        context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<List<Usuario>> ListarAsync(CancellationToken cancellationToken = default) =>
        context.Usuarios.AsNoTracking().OrderBy(u => u.Nome).ToListAsync(cancellationToken);

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await context.Usuarios.AddAsync(usuario, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        context.Usuarios.Update(usuario);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (usuario is null) return;
        context.Usuarios.Remove(usuario);
        await context.SaveChangesAsync(cancellationToken);
    }
}
