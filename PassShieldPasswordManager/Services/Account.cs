using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services;

public class Account
{
    private readonly IUserRepo _userRepo;
    private readonly ICredentialRepo _credentialRepo;
    private readonly Mapper _mapper = new Mapper();
    private readonly LoginSession _loginSession = LoginSession.Instance;

    public Account(IUserRepo userRepo, ICredentialRepo credentialRepo)
    {
        _userRepo = userRepo;
        _credentialRepo = credentialRepo;
    }

    public async Task<User> Login(string username, string password)
    {
        try
        {
            var users = await _userRepo.Login(username, password);
            if (users != null)
            {
                if (users.IsAdmin)
                {
                    var admin = new Admin(_userRepo, _credentialRepo);
                    return _mapper.MapToUser(users, admin);
                }
                else
                {
                    var user = new User(_credentialRepo);
                    return _mapper.MapToUser(users, user);
                }
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
            var users = _mapper.MapToUserModel(user, new UserModel());
            users.Password = new Encryption(users.Password).CreateSha512();
            return _mapper.MapToUser(await _userRepo.Add(users), user);
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
            var users = await _userRepo.GetByUsername(username);
            if (users != null)
            {
                
                if (users.IsAdmin)
                {
                    var admin = new Admin(_userRepo, _credentialRepo);
                    return _mapper.MapToUser(users, admin);
                }
                else
                {
                    var user = new User(_credentialRepo);
                    return _mapper.MapToUser(users, user);
                }
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
