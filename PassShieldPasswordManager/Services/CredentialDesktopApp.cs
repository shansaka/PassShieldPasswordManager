using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services
{
    public class CredentialDesktopApp : Credential, ICredential
    {
        // Property
        public string DesktopAppName { get; set; }
        
        // Dependency
        private readonly ICredentialRepo _credentialRepo;
        
        // Constructor with repository injection
        public CredentialDesktopApp(ICredentialRepo credentialRepo) : base(credentialRepo)
        {
            _credentialRepo = credentialRepo;
        }
        
        // Method to add a desktop application credential
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
        
        // Method to edit a desktop application credential
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
}
