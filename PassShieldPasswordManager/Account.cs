using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
namespace PassShieldPasswordManager;

public class Account
{
    private readonly UserRepo _userRepo = new();
    public User User { get; set; }
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;

    public async Task<User> Login(string username, string password)
    {
        try
        {
            var user = await _userRepo.Login(username, password);
            return user.IsAdmin ? _mapper.Map<Admin>(user) : _mapper.Map<User>(user);
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
}