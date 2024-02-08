using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class CredentialWebsite : Credential, ICredential
{
    public string WebsiteName { get; set; }
    public string Url { get; set; }
    
    private readonly ICredentialRepo _credentialRepo;

    public CredentialWebsite(ICredentialRepo credentialRepo) : base(credentialRepo)
    {
        _credentialRepo = credentialRepo;
    }

    public async Task Add()
    {
        try
        {
            var credentials = new CredentialModel
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

    public async Task Edit()
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