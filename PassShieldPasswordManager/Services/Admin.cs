using AutoMapper;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services;

public class Admin : User
{
    private readonly ICredentialRepo _credentialRepo;
    private readonly Credential _credential;
    private readonly IUserRepo _userRepo;
    private readonly Mapper _mapper = new Mapper();

    public Admin(IUserRepo userRepo, ICredentialRepo credentialRepo) : base(credentialRepo)
    {
        _userRepo = userRepo;
        _credentialRepo = credentialRepo;
        _credential = new Credential(_credentialRepo);
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
        foreach (var userModel in users)
        {
                
            if (userModel.IsAdmin)
            {
                var admin = new Admin(_userRepo, _credentialRepo);
                returnList.Add(_mapper.MapToUser(userModel, admin));
            }
            else
            {
                var user = new User(_credentialRepo);
                returnList.Add(_mapper.MapToUser(userModel, user));
            }
            
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