using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services;

public class Credential : ICredential
{
    public int CredentialId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public User User { get; set; }
    
    private readonly ICredentialRepo _credentialRepo;
    //private readonly IMapper _mapper;
    private readonly Mapper _mapper = new Mapper();
    public Credential(ICredentialRepo credentialRepo)
    {
        _credentialRepo = credentialRepo;
    }

    public async Task<List<Credential>> GetList(User user, string name = null)
    {
        try
        {
            var result = await _credentialRepo.GetAllByUserId(user.UserId, name);
            return MapCredentialTypes(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Credential>> GetList(Admin admin)
    {
        try
        {
            var result = await _credentialRepo.GetAll();
            return MapCredentialTypes(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(int credentialId)
    {
        try
        {
            var credentials = await _credentialRepo.GetById(credentialId);
            await _credentialRepo.Delete(credentials);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<Credential> MapCredentialTypes(IEnumerable<CredentialModel> credentials)
    {
        var list = new List<Credential>();
        foreach (var item in credentials)
        {
            switch (item.Type)
            {
                case (int)CredentialType.Game:
                    var credentialGame = new CredentialGame(_credentialRepo);
                    credentialGame = _mapper.MapToCredential(item, credentialGame);
                    credentialGame.GameName = item.Name;
                    credentialGame.Developer = item.UrlOrDeveloper;
                    list.Add(credentialGame);
                    break;
                case (int)CredentialType.Website:
                    var credentialWebsite = new CredentialWebsite(_credentialRepo);
                    credentialWebsite = _mapper.MapToCredential(item, credentialWebsite);
                    credentialWebsite.WebsiteName = item.Name;
                    credentialWebsite.Url = item.UrlOrDeveloper;
                    list.Add(credentialWebsite);
                    break;
                case (int)CredentialType.DesktopApp:
                    var credentialDesktopApp = new CredentialDesktopApp(_credentialRepo);
                    credentialDesktopApp = _mapper.MapToCredential(item, credentialDesktopApp);
                    credentialDesktopApp.DesktopAppName = item.Name;
                    list.Add(credentialDesktopApp);
                    break;
            }
        }
        return list;
    }

    public string GenerateRandomPassword(RandomPasswordGenerator passwordGenerator)
    {
        return passwordGenerator.Generate();
    }

    public Task Add()
    {
        throw new NotImplementedException();
    }

    public Task Edit()
    {
        throw new NotImplementedException();
    }
}