using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services
{
    public class CredentialGame : Credential, ICredential
    {
        // Properties
        public string GameName { get; set; }
        public string Developer { get; set; }
        
        // Dependency
        private readonly ICredentialRepo _credentialRepo;

        // Constructor with repository injection
        public CredentialGame(ICredentialRepo credentialRepo) : base(credentialRepo)
        {
            _credentialRepo = credentialRepo;
        }

        // Method to add a game credential
        public async Task Add()
        {
            try
            {
                var credentials = new CredentialModel
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

        // Method to edit a game credential
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
}
