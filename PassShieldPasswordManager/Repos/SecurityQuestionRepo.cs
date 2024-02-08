using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Services;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos;

public class SecurityQuestionRepo : ISecurityQuestionRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;

    #region Common Methods

    public async Task<SecurityQuestionModel> GetById(int id)
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

    public async Task<IEnumerable<SecurityQuestionModel>> GetAll()
    {
        try
        {
            var result = await _dbConnection.Database.SecurityQuestions.ToListAsync();
            if (!result.Any())
            {
                var question1 = new SecurityQuestionModel
                {
                    Question = "What's your first pet name?"
                };
                _dbConnection.Database.SecurityQuestions.Add(question1);
                await _dbConnection.Database.SaveChangesAsync();
                
                var question2 = new SecurityQuestionModel
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

    #endregion
}