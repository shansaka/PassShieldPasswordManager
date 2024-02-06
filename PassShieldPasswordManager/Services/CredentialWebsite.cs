using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class CredentialWebsite : Credential
{
    public string WebsiteName { get; set; }
    public string Url { get; set; }
    
    private readonly CredentialRepo _credentialRepo;

    public CredentialWebsite()
    {
        _credentialRepo = new CredentialRepo();
    }

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
                Type = (int)CredentialType.Website,
                CreatedDate = DateTime.Now
            };
            await _credentialRepo.Add(credentials);
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
            var credentials = await _credentialRepo.GetById(CredentialId);
            if (credentials != null)
            {
                credentials.Username = Username;
                credentials.Password = new Encryption(Password).Encrypt();
                credentials.Name = WebsiteName;
                credentials.UrlOrDeveloper = Url;
                credentials.UpdatedDate = DateTime.Now;
                await _credentialRepo.Update(credentials);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}