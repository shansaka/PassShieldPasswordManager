using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class Account
{
    private readonly UserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly LoginSession _loginSession;

    public Account()
    {
        _mapper = AutoMapperConfiguration.Instance.Mapper;
        _loginSession = LoginSession.Instance;
        _userRepo = new UserRepo();
    }

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
            users.Password = new Encryption(users.Password).CreateSha512();
            return _mapper.Map<User>(await _userRepo.Add(users));
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
            var user = await _userRepo.GetByUsername(username);
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
            var user = await _userRepo.GetById(userId);
            if (user != null)
            {
                user.Password = new Encryption(newPassword).CreateSha512();
                await _userRepo.Update(user);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Logout()
    {
        _loginSession.Logout();
    }
}