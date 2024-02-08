using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos.Interfaces;

public interface ICredentialRepo : IRepository<CredentialModel>
{
    Task<List<CredentialModel>> GetAllByUserId(int userId, string name = null);
}