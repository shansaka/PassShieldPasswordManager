using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Services;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos;

public class SecurityQuestionRepo : IRepository<SecurityQuestions>
{
    private readonly DbConnection _dbConnection;

    public SecurityQuestionRepo()
    {
        _dbConnection = DbConnection.Instance;
    }

    #region Common Methods

    public async Task<SecurityQuestions> GetById(int id)
    {
        try
        {
            return await _dbConnection.Database.SecurityQuestions.FirstOrDefaultAsync(x => x.SecurityQuestionId == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<SecurityQuestions>> GetAll()
    {
        try
        {
            var result = await _dbConnection.Database.SecurityQuestions.ToListAsync();
            if (!result.Any())
            {
                var question1 = new SecurityQuestion
                {
                    Question = "What's your first pet name?"
                };
                _dbConnection.Database.SecurityQuestions.Add(question1);
                await _dbConnection.Database.SaveChangesAsync();
                
                var question2 = new SecurityQuestion
                {
                    Question = "What city you have born?"
                };
                _dbConnection.Database.SecurityQuestions.Add(question2);
                await _dbConnection.Database.SaveChangesAsync();
                
                result = await _dbConnection.Database.SecurityQuestions.ToListAsync();
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<SecurityQuestions> Add(SecurityQuestions entity)
    {
        try
        {
            await _dbConnection.Database.SecurityQuestions.AddAsync(entity);
            await _dbConnection.Database.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Update(SecurityQuestions entity)
    {
        try
        {
            _dbConnection.Database.SecurityQuestions.Update(entity);
            await _dbConnection.Database.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(SecurityQuestions entity)
    {
        try
        {
            _dbConnection.Database.SecurityQuestions.Remove(entity);
            await _dbConnection.Database.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
}