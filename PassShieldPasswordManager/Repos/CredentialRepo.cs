using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;

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
    
    public async Task<List<Credentials>> GetCredentialList(int userId)
    {
        try
        {
            return await _dbConnection.Db.Credentials.Where(x => x.UserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}