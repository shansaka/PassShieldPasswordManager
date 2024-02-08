namespace PassShieldPasswordManager.Repos.Interfaces;

public interface IRepositoryWriterAndRemover<T>
{
    Task<T> Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}