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
            return _mapper.Map<User>(await _userRepo.CreateUser(user));
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
            return await _userRepo.VerifyUsername(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}