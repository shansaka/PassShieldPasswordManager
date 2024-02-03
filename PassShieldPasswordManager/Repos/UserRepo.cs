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
            // var query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";
            // var result = await _dbConnection.Db.QueryFirstOrDefaultAsync<UserModel>(query,
            //     new
            //     {
            //         Username = username,
            //         Password = password
            //     });

            var result =
                await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<User> CreateUser(User user)
    {
        try
        {
            // var query = "INSERT INTO Users (Name, Username, Password, SecurityQuestionId, SecurityAnswer) VALUES @Name, @Username, @Password, @SecurityQuestionId, @SecurityAnswer, @DateCreated";
            // var result = await _dbConnection.Db.ExecuteScalarAsync<UserModel>(query, user);
            //return result;
            _dbConnection.Db.Users.Add(user);
            var id = await _dbConnection.Db.SaveChangesAsync();
            user.UserId = id;
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> VerifyUsername(string username)
    {
        try
        {
            var result = await _dbConnection.Db.Users.FirstOrDefaultAsync(x => x.Username == username);
            return result != null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}