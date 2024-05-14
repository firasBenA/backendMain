// Interfaces/IRepository.cs
public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    void Add(T entity);
}
