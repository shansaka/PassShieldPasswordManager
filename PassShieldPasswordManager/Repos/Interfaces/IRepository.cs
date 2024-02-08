namespace PassShieldPasswordManager.Repos.Interfaces;

public interface IRepository<T> : IRepositoryReader<T>, IRepositoryWriterAndRemover<T>
{
    
}
