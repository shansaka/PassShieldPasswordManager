using AutoMapper;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager;

public class Admin : User
{

    private readonly Credential _credential = new();
    private readonly UserRepo _userRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;
    public async Task<List<Credential>> ViewAllCredentials()
    {
        var credentialsList = await _credential.GetList(this);
        return credentialsList;
    }
    
    public async Task<List<User>> ViewUsers()
    {
        var returnList = new List<User>();
        var users = await _userRepo.GetUsers();
        foreach (var user in users)
        {
            returnList.Add(user.IsAdmin ? _mapper.Map<Admin>(user) : _mapper.Map<User>(user));
        }
        return returnList;
    }

    public async Task MakeUserAdmin(int userId)
    {
        await _userRepo.MakeUserAdmin(userId);
    }

    public async Task DeleteUser(int userId)
    {
        await _userRepo.DeleteUser(userId);
    }
}