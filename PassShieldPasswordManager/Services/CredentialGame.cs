using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class CredentialGame : Credential, ICredential
{
    public string GameName { get; set; }
    public string Developer { get; set; }
    
    private readonly CredentialRepo _credentialRepo;

    public CredentialGame()
    {
        _credentialRepo = new CredentialRepo();
    }

    public async Task Add()
    {
        try
        {
            var credentials = new Credentials
            {
                Username = Username,
                Password = new Encryption(Password).Encrypt(),
                Name = GameName,
                UrlOrDeveloper = Developer,
                UserId = User.UserId,
                Type = (int)CredentialType.Game,
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
                credentials.Name = GameName;
                credentials.UrlOrDeveloper = Developer;
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