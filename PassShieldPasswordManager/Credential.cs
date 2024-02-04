using System.Runtime.InteropServices;
using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;

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
    
    public async Task<List<Credential>> GetList(int userId)
    {
        try
        {
            var list = new List<Credential>();
            var result = await _credentialRepo.GetCredentialList(userId);
            foreach (var item in result)
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}