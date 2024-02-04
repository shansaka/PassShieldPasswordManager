using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;

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
                Password = Password,
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
}