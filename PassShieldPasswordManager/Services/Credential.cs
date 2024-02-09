using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services
{
    public class Credential
    {
        // Properties
        public int CredentialId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public User User { get; set; }
        
        // Dependencies
        private readonly ICredentialRepo _credentialRepo;
        private readonly Mapper _mapper = new (); // Utilized for mapping between domain and view models
        
        // Constructor with repository injection
        public Credential(ICredentialRepo credentialRepo)
        {
            _credentialRepo = credentialRepo;
        }

        // Method to get a list of credentials for a user
        public async Task<List<Credential>> GetList(User user, string name = null)
        {
            try
            {
                var result = await _credentialRepo.GetAllByUserId(user.UserId, name);
                return MapCredentialTypes(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to get a list of credentials for an admin
        public async Task<List<Credential>> GetList(Admin admin)
        {
            try
            {
                var result = await _credentialRepo.GetAll();
                return MapCredentialTypes(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Method to delete a credential
        public async Task Delete(int credentialId)
        {
            try
            {
                var credentials = await _credentialRepo.GetById(credentialId);
                await _credentialRepo.Delete(credentials);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to generate a random password
        public string GenerateRandomPassword(RandomPasswordGenerator passwordGenerator)
        {
            return passwordGenerator.Generate();
        }

        // Method to add a credential
        public async Task Add(ICredential credential)
        {
            await credential.Add();
        }

        // Method to edit a credential
        public async Task Edit(ICredential credential)
        {
            await credential.Edit();
        }
        
        // Method to map different credential types
        private List<Credential> MapCredentialTypes(IEnumerable<CredentialModel> credentials)
        {
            var list = new List<Credential>();
            foreach (var item in credentials)
            {
                switch (item.Type)
                {
                    case (int)CredentialType.Game:
                        var credentialGame = new CredentialGame(_credentialRepo);
                        credentialGame = _mapper.MapToCredential(item, credentialGame);
                        credentialGame.GameName = item.Name;
                        credentialGame.Developer = item.UrlOrDeveloper;
                        credentialGame.User = _mapper.MapToUser(item.User, new User(_credentialRepo));
                        list.Add(credentialGame);
                        break;
                    case (int)CredentialType.Website:
                        var credentialWebsite = new CredentialWebsite(_credentialRepo);
                        credentialWebsite = _mapper.MapToCredential(item, credentialWebsite);
                        credentialWebsite.WebsiteName = item.Name;
                        credentialWebsite.Url = item.UrlOrDeveloper;
                        credentialWebsite.User = _mapper.MapToUser(item.User, new User(_credentialRepo));
                        list.Add(credentialWebsite);
                        break;
                    case (int)CredentialType.DesktopApp:
                        var credentialDesktopApp = new CredentialDesktopApp(_credentialRepo);
                        credentialDesktopApp = _mapper.MapToCredential(item, credentialDesktopApp);
                        credentialDesktopApp.DesktopAppName = item.Name;
                        credentialDesktopApp.User = _mapper.MapToUser(item.User, new User(_credentialRepo));
                        list.Add(credentialDesktopApp);
                        break;
                }
            }
            return list;
        }
    }
}
