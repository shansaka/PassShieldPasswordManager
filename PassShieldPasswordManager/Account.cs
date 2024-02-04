using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
namespace PassShieldPasswordManager;

public class Account
{
    private readonly UserRepo _userRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;

    public async Task<User> Login(string username, string password)
    {
        try
        {
            var user = await _userRepo.Login(username, password);
            if (user != null)
            {
                return user.IsAdmin ? _mapper.Map<Admin>(user) : _mapper.Map<User>(user);
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<User> Register(User user)
    {
        try
        {
            var users = _mapper.Map<Users>(user);
            return _mapper.Map<User>(await _userRepo.CreateUser(users));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<User> VerifyUsername(string username)
    {
        try
        {
            return _mapper.Map<User>(await _userRepo.GetByUsername(username));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> VerifySecurityAnswer(int userId, int securityQuestionId, string securityAnswer)
    {
        try
        {
            return await _userRepo.VerifySecurityAnswer(userId, securityQuestionId, securityAnswer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ResetPassword(int userId, string newPassword)
    {
        try
        {
            await _userRepo.ResetPassword(userId, newPassword);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}