public interface IWriteRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken ct);
    Task UpdateAsync(TEntity entity, CancellationToken ct);
    Task DeleteAsync(TEntity entity, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}