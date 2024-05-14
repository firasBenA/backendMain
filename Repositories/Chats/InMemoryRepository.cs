// Repositories/InMemoryRepository.cs
using System.Collections.Generic;
using System.Linq;

public class InMemoryRepository<T> : IRepository<T>
{
    private readonly List<T> _entities = new List<T>();

    public IEnumerable<T> GetAll()
    {
        return _entities.ToList();
    }

    public void Add(T entity)
    {
        _entities.Add(entity);
    }
}
