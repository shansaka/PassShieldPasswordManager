using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos
{
    // Repository for managing credentials
    public class CredentialRepo : ICredentialRepo
    {
        // Dependency
        private readonly DbConnection _dbConnection = DbConnection.Instance;
        
        #region Common Methods
        
        // Retrieve a credential by its ID
        public async Task<CredentialModel> GetById(int id)
        {
            try
            {
                return await _dbConnection.Database.Credentials.FirstOrDefaultAsync(x => x.CredentialId == id && x.IsDeleted == false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Retrieve all credentials
        public async Task<IEnumerable<CredentialModel>> GetAll()
        {
            try
            {
                return await _dbConnection.Database.Credentials.Include(x => x.User).Where(x => x.IsDeleted == false).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Add a new credential
        public async Task<CredentialModel> Add(CredentialModel entity)
        {
            try
            {
                await _dbConnection.Database.Credentials.AddAsync(entity);
                await _dbConnection.Database.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Update an existing credential
        public async Task Update(CredentialModel entity)
        {
            try
            {
                _dbConnection.Database.Credentials.Update(entity);
                await _dbConnection.Database.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Delete a credential
        public async Task Delete(CredentialModel entity)
        {
            try
            {
                entity.IsDeleted = true;
                _dbConnection.Database.Credentials.Update(entity);
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

        // Retrieve all credentials by user ID
        public async Task<List<CredentialModel>> GetAllByUserId(int userId, string name = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    return await _dbConnection.Database.Credentials.Where(x =>
                        x.UserId == userId &&
                        x.IsDeleted == false &&
                        x.Name.Contains(name)).ToListAsync();
                }
                return await _dbConnection.Database.Credentials.Where(x => x.UserId == userId && x.IsDeleted == false).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
