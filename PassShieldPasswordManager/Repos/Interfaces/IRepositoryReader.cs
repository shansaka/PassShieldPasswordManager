namespace PassShieldPasswordManager.Repos.Interfaces;

public interface IRepositoryReader<T>
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
}