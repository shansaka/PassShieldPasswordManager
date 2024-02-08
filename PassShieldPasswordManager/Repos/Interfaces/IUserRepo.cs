using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos.Interfaces;

public interface IUserRepo : IRepository<UserModel>
{
    Task<UserModel> Login(string username, string password);
    Task<UserModel> GetByUsername(string username);
    Task<bool> VerifySecurityAnswer(int userId, int securityQuestionId, string securityAnswer);
}