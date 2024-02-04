using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager;

public class CredentialDesktopApp : Credential
{
    public string DesktopAppName { get; set; }
    
    private readonly CredentialRepo _credentialRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;


    public async Task Create()
    {
        try
        {
            var credentials = new Credentials
            {
                UserId = User.UserId,
                Username = Username,
                Password = new Encryption(Password).Encrypt(),
                Name = DesktopAppName,
                Type = (int)CredentialType.DesktopApp
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
                Name = DesktopAppName,
                UserId = User.UserId,
                Type = (int)CredentialType.DesktopApp
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