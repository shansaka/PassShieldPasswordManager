using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos;

public class UserRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;
    
    public async Task<Users> Login(string username, string password)
    {
        try
        {
            // Encrypting the password
            password = new Encryption(password).CreateSha512();
            
            var result =
                await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Users> CreateUser(Users user)
    {
        try
        {
            user.Password = new Encryption(user.Password).CreateSha512();
            await _dbConnection.Db.Users.AddAsync(user);
            var id = await _dbConnection.Db.SaveChangesAsync();
            user.UserId = id;
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Users> GetByUsername(string username)
    {
        try
        {
            var result = await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.Username == username);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> VerifySecurityAnswer(int userId, int securityQuestionId, string securityAnswer)
    {
        try
        {
            var result = await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.UserId == userId && x.SecurityQuestionId == securityQuestionId);
            return result != null && result.SecurityAnswer == securityAnswer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ResetPassword(int userId, string newPassword)
    {
        try
        {
            var result = await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (result != null)
            {
                result.Password = new Encryption(newPassword).CreateSha512();
                await _dbConnection.Db.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}