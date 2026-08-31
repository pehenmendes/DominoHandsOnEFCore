namespace DominoPontaDeQuina.Repository.Repositories;

public interface IUnitOfWork
{
    Task<int> SalvarAsync(CancellationToken cancellationToken = default);
}
