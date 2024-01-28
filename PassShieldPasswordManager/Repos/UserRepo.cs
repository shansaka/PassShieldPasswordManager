using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Repos;

public class UserRepo
{
    private readonly DbConnection _dbConnection = DbConnection.Instance;
    
    public async Task<UserModel> Login(string username, string password)
    {
        try
        {
            var query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";
            var result = await _dbConnection.Db.QueryFirstOrDefaultAsync<UserModel>(query,
                new
                {
                    Username = username,
                    Password = password
                });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserModel> CreateUser(User user)
    {
        try
        {
            var query = "INSERT INTO Users (Name, Username, Password, SecurityQuestionId, SecurityAnswer) VALUES @Name, @Username, @Password, @SecurityQuestionId, @SecurityAnswer, @DateCreated";
            var result = await _dbConnection.Db.ExecuteScalarAsync<UserModel>(query, user);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}