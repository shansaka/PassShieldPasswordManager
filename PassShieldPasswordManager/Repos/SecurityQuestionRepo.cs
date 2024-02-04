using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos;

public class SecurityQuestionRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;
    
    public async Task<List<SecurityQuestions>> GetList()
    {
        try
        {
            var result = await _dbConnection.Db.SecurityQuestions.ToListAsync();
            if (!result.Any())
            {
                var question1 = new SecurityQuestion
                {
                    Question = "What's your first pet name?"
                };
                _dbConnection.Db.SecurityQuestions.Add(question1);
                await _dbConnection.Db.SaveChangesAsync();
                
                var question2 = new SecurityQuestion
                {
                    Question = "What city you have born?"
                };
                _dbConnection.Db.SecurityQuestions.Add(question2);
                await _dbConnection.Db.SaveChangesAsync();
                
                result = await _dbConnection.Db.SecurityQuestions.ToListAsync();
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<SecurityQuestions> GetById(int id)
    {
        try
        {
            return await _dbConnection.Db.SecurityQuestions.FirstOrDefaultAsync(x => x.SecurityQuestionId == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}