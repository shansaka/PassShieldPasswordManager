using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Services;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Repos
{
    // Repository for managing security questions
    public class SecurityQuestionRepo : ISecurityQuestionRepo
    {
        // Dependency
        private readonly DbConnection _dbConnection = DbConnection.Instance;

        #region Common Methods

        // Retrieve a security question by its ID
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

        // Retrieve all security questions
        public async Task<IEnumerable<SecurityQuestionModel>> GetAll()
        {
            try
            {
                var result = await _dbConnection.Database.SecurityQuestions.ToListAsync();
                if (!result.Any())
                {
                    // If there are no security questions in the database, add default ones
                    var question1 = new SecurityQuestionModel
                    {
                        Question = "What's your first pet's name?"
                    };
                    _dbConnection.Database.SecurityQuestions.Add(question1);
                    await _dbConnection.Database.SaveChangesAsync();
                    
                    var question2 = new SecurityQuestionModel
                    {
                        Question = "What city were you born in?"
                    };
                    _dbConnection.Database.SecurityQuestions.Add(question2);
                    await _dbConnection.Database.SaveChangesAsync();
                    
                    // Fetch the updated list of security questions
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
}
