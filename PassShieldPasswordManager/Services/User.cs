using AutoMapper;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services
{
    public class User
    {
        // Properties
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SecurityQuestionId { get; set; }
        public string SecurityAnswer { get; set; }
        public DateTime DateCreated { get; set; }
        
        // Dependency
        private readonly Credential _credential;
        
        // Constructor with repository injection
        public User(ICredentialRepo credentialRepo)
        {
            _credential = new Credential(credentialRepo);
        }

        // Method to edit a credential
        public async Task EditCredential(ICredential credential)
        {
            try
            {
                await _credential.Edit(credential);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to add a credential
        public async Task AddCredential(ICredential credential)
        {
            try
            {
                await _credential.Add(credential);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to view credentials
        public async Task<List<Credential>> ViewCredentials(SortBy sortBy = SortBy.None, SortOrder sortOrder = SortOrder.None, string search = null)
        {
            var credentialsList = await _credential.GetList(this, search);
            
            // Sorting if passed
            switch (sortBy)
            {
                case SortBy.Username:
                    credentialsList = sortOrder == SortOrder.Ascending
                        ? credentialsList.OrderBy(x => x.Username).ToList()
                        : credentialsList.OrderByDescending(x => x.Username).ToList();
                    break;
                case SortBy.CreatedDate:
                    credentialsList = sortOrder == SortOrder.Ascending
                        ? credentialsList.OrderBy(x => x.CreatedDate).ToList()
                        : credentialsList.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
                case SortBy.UpdatedDate:
                    credentialsList = sortOrder == SortOrder.Ascending
                        ? credentialsList.OrderBy(x => x.UpdatedDate).ToList()
                        : credentialsList.OrderByDescending(x => x.UpdatedDate).ToList();
                    break;
            }

            return credentialsList;
        }

        // Method to delete a credential
        public async Task DeleteCredential(int credentialId)
        {
            await _credential.Delete(credentialId);
        }
    }
}
