using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos;

public class SecurityQuestionRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;
    
    public async Task<List<SecurityQuestionModel>> GetList()
    {
        try
        {
            // var query = "SELECT * FROM SecurityQuestions";
            // var result = await _dbConnection.Db.QueryFirstOrDefaultAsync<List<SecurityQuestionModel>>(query);
            // if (result == null || !result.Any())
            // {
            //     query = "INSERT INTO SecurityQuestions (Question) VALUES (@Question)";
            //     await _dbConnection.Db.ExecuteAsync(query, new
            //     {
            //         Question = "What's your first pet name?"
            //     });
            //     
            //     await _dbConnection.Db.ExecuteAsync(query, new
            //     {
            //         Question = "What city you have born?"
            //     });
            //     
            //     result = await _dbConnection.Db.QueryFirstOrDefaultAsync<List<SecurityQuestionModel>>(query);ß
            // }
            // return result;

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
}