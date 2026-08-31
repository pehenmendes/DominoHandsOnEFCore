using DominoPontaDeQuina.Repository.Context;

namespace DominoPontaDeQuina.Repository.Repositories;

public class UnitOfWork(DominoDbContext context) : IUnitOfWork
{
    public Task<int> SalvarAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
