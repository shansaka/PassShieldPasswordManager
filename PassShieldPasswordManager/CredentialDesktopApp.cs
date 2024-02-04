using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;

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
                Username = Username,
                Password = Password,
                Name = DesktopAppName,
                UserId = User.UserId,
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
}