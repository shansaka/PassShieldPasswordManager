using AutoMapper;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class Admin : User
{

    private readonly Credential _credential;
    private readonly UserRepo _userRepo;
    private readonly IMapper _mapper;

    public Admin()
    {
        _userRepo = new UserRepo();
        _mapper = AutoMapperConfiguration.Instance.Mapper;
        _credential = new Credential();
    }

    public async Task<List<Credential>> ViewAllCredentials()
    {
        var credentialsList = await _credential.GetList(this);
        return credentialsList;
    }
    
    public async Task<List<User>> ViewUsers()
    {
        var returnList = new List<User>();
        var users = await _userRepo.GetAll();
        foreach (var user in users)
        {
            returnList.Add(user.IsAdmin ? _mapper.Map<Admin>(user) : _mapper.Map<User>(user));
        }
        return returnList;
    }

    public async Task MakeUserAdmin(int userId)
    {
        var user = await _userRepo.GetById(userId);
        if (user != null)
        {
            user.IsAdmin = true;
            await _userRepo.Update(user);
        }
    }

    public async Task DeleteUser(int userId)
    {
        var user = await _userRepo.GetById(userId);
        await _userRepo.Delete(user);
    }
}