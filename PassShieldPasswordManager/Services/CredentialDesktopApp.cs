using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class CredentialDesktopApp : Credential
{
    public string DesktopAppName { get; set; }
    
    private readonly ICredentialRepo _credentialRepo;
    
    public CredentialDesktopApp(ICredentialRepo credentialRepo) : base(credentialRepo)
    {
        _credentialRepo = credentialRepo;
    }
    
    public async Task Add()
    {
        try
        {
            var credentials = new CredentialModel
            {
                UserId = User.UserId,
                Username = Username,
                Password = new Encryption(Password).Encrypt(),
                Name = DesktopAppName,
                Type = (int)CredentialType.DesktopApp,
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
                credentials.Name = DesktopAppName;
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