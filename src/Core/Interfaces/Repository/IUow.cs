namespace Core.Interfaces.Repository;

public interface IUow
{
    Task<int> CommitAsync(CancellationToken cancellationToken);


}