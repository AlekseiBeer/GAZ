public interface IReadRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(CancellationToken ct);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct);
    IQueryable<TEntity> Query();           // чтобы строить сложные фильтры/пагинацию
}