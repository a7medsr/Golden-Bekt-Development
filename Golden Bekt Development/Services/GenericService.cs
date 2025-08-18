public class GenericService<T> where T : class
{
    private readonly IRepository<T> _repository;

    public GenericService(IRepository<T> repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<T>> GetAllAsync() => _repository.GetAllAsync();
    public Task<T?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);
    public Task AddAsync(T entity) => _repository.AddAsync(entity);
    public Task UpdateAsync(T entity) => _repository.UpdateAsync(entity);
    public Task DeleteAsync(string id) => _repository.DeleteAsync(id);
}
