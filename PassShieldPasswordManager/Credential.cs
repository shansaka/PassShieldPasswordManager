using System.Runtime.InteropServices;
using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager;

public class Credential
{
    public int CredentialId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public User User { get; set; }
    
    private readonly CredentialRepo _credentialRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;
    
    public async Task<List<Credential>> GetList(User user, string name = null)
    {
        try
        {
            
            var result = await _credentialRepo.GetCredentialList(user.UserId, name);
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
            var result = await _credentialRepo.GetCredentialList();
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
            await _credentialRepo.DeleteCredential(credentialId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<Credential> MapCredentialTypes(List<Credentials> credentials)
    {
        var list = new List<Credential>();
        foreach (var item in credentials)
        {
            switch (item.Type)
            {
                case (int)CredentialType.Game:
                    var credentialGame = _mapper.Map<CredentialGame>(item);
                    credentialGame.GameName = item.Name;
                    credentialGame.Developer = item.UrlOrDeveloper;
                    list.Add(credentialGame);
                    break;
                case (int)CredentialType.Website:
                    var credentialWebsite = _mapper.Map<CredentialWebsite>(item);
                    credentialWebsite.WebsiteName = item.Name;
                    credentialWebsite.Url = item.UrlOrDeveloper;
                    list.Add(credentialWebsite);
                    break;
                case (int)CredentialType.DesktopApp:
                    var credentialDesktopApp = _mapper.Map<CredentialDesktopApp>(item);
                    credentialDesktopApp.DesktopAppName = item.Name;
                    list.Add(credentialDesktopApp);
                    break;
            }
        }
        return list;
    }
}