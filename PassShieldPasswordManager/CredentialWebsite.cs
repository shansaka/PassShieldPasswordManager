using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager;

public class CredentialWebsite : Credential
{
    public string WebsiteName { get; set; }
    public string Url { get; set; }
    
    private readonly CredentialRepo _credentialRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;
    
    public async Task Create()
    {
        try
        {
            var credentials = new Credentials
            {
                Username = Username,
                Password = new Encryption(Password).Encrypt(),
                Name = WebsiteName,
                UrlOrDeveloper = Url,
                UserId = User.UserId,
                Type = (int)CredentialType.Website
            };
            await _credentialRepo.CreateCredential(credentials);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Update()
    {
        try
        {
            var credentials = new Credentials
            {
                CredentialId = CredentialId,
                Username = Username,
                Password = new Encryption(Password).Encrypt(),
                Name = WebsiteName,
                UrlOrDeveloper = Url,
                Type = (int)CredentialType.Website
            };
            await _credentialRepo.UpdateCredential(credentials);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}