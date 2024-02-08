using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class CredentialDesktopApp : Credential, ICredential
{
    public string DesktopAppName { get; set; }
    
    private readonly CredentialRepo _credentialRepo;

    public CredentialDesktopApp()
    {
        _credentialRepo = new CredentialRepo();
    }


    public async Task Add()
    {
        try
        {
            var credentials = new Credentials
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