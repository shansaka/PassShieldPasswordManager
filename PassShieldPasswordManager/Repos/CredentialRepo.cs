using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos;

public class CredentialRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;
    
    public async Task CreateCredential(Credentials credentials)
    {
        try
        {
            credentials.CreatedDate = DateTime.Now;
            await _dbConnection.Db.Credentials.AddAsync(credentials);
            await _dbConnection.Db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Credentials>> GetCredentialList(int userId, string name = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(name))
            {
                return await _dbConnection.Db.Credentials.Where(x => 
                    x.UserId == userId && 
                    x.IsDeleted == 0 && 
                    x.Name.Contains(name)).ToListAsync();
            }
            return await _dbConnection.Db.Credentials.Where(x => x.UserId == userId && x.IsDeleted == 0).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Credentials>> GetCredentialList()
    {
        try
        {
            return await _dbConnection.Db.Credentials.Include(x => x.User).Where(x => x.IsDeleted == 0).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateCredential(Credentials credentials)
    {
        try
        {
            var obj = await _dbConnection.Db.Credentials.FirstOrDefaultAsync(x => x.CredentialId == credentials.CredentialId);
            if (obj != null)
            {
                obj.Username = credentials.Username;
                obj.Password = credentials.Password;
                obj.Name = credentials.Name;
                obj.UrlOrDeveloper = credentials.UrlOrDeveloper;
                obj.UpdatedDate = DateTime.Now;
                await _dbConnection.Db.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteCredential(int credentialId)
    {
        try
        {
            var obj = await _dbConnection.Db.Credentials.FirstOrDefaultAsync(x => x.CredentialId == credentialId);
            if (obj != null)
            {
                obj.IsDeleted = 1;
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