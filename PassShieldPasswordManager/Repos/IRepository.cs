using System;
namespace PassShieldPasswordManager.Repos;

public interface IRepository<T>
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
