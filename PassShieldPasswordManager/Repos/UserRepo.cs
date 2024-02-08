using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos;

public class UserRepo : IUserRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;

    #region Common Methods

    public async Task<UserModel> GetById(int id)
    {
        try
        {
            return await _dbConnection.Database.Users.FirstOrDefaultAsync(x => x.UserId == id && x.IsDeleted == false );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<UserModel>> GetAll()
    {
        try
        {
            return await _dbConnection.Database.Users.Where(x => x.IsDeleted == false).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserModel> Add(UserModel entity)
    {
        try
        {
            await _dbConnection.Database.Users.AddAsync(entity);
            await _dbConnection.Database.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Update(UserModel entity)
    {
        try
        {
            _dbConnection.Database.Users.Update(entity);
            await _dbConnection.Database.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(UserModel entity)
    {
        try
        {
            entity.IsDeleted = true;
            _dbConnection.Database.Users.Update(entity);
            await _dbConnection.Database.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
    
    #region Custom Methods

    public async Task<UserModel> Login(string username, string password)
    {
        try
        {
            // Encrypting the password
            password = new Encryption(password).CreateSha512();
            
            var result =
                await _dbConnection.Database.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserModel> GetByUsername(string username)
    {
        try
        {
            var result = await _dbConnection.Database.Users.FirstOrDefaultAsync(x => x.Username == username);
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
            var result = await _dbConnection.Database.Users.FirstOrDefaultAsync(x => x.UserId == userId && x.SecurityQuestionId == securityQuestionId);
            return result != null && result.SecurityAnswer == securityAnswer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
    
}