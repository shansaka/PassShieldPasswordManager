using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager;

public class CredentialGame : Credential
{
    public string GameName { get; set; }
    public string Developer { get; set; }
    
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
                Name = GameName,
                UrlOrDeveloper = Developer,
                UserId = User.UserId,
                Type = (int)CredentialType.Game
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
                Name = GameName,
                UrlOrDeveloper = Developer,
                Type = (int)CredentialType.Game
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