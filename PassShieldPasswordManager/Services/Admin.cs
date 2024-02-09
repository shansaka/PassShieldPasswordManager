using AutoMapper;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services
{
    public class Admin : User
    {
        // Dependencies
        private readonly ICredentialRepo _credentialRepo;
        private readonly Credential _credential;
        private readonly IUserRepo _userRepo;
        private readonly Mapper _mapper = new (); // Utilized for mapping between domain and view models

        // Constructor with repository injection
        public Admin(IUserRepo userRepo, ICredentialRepo credentialRepo) : base(credentialRepo)
        {
            _userRepo = userRepo;
            _credentialRepo = credentialRepo;
            _credential = new Credential(_credentialRepo);
        }
        
        // Method to view all credentials
        public async Task<List<Credential>> ViewAllCredentials()
        {
            // Getting list of credentials
            var credentialsList = await _credential.GetList(this);
            return credentialsList;
        }
        
        // Method to view all users
        public async Task<List<User>> ViewUsers()
        {
            var returnList = new List<User>();
            // Getting all users
            var users = await _userRepo.GetAll();
            foreach (var userModel in users)
            {
                // Checking if the user is an admin
                if (userModel.IsAdmin)
                {
                    var admin = new Admin(_userRepo, _credentialRepo);
                    returnList.Add(_mapper.MapToUser(userModel, admin));
                }
                else
                {
                    var user = new User(_credentialRepo);
                    returnList.Add(_mapper.MapToUser(userModel, user));
                }
            }
            return returnList;
        }

        // Method to make a user an admin
        public async Task MakeUserAdmin(int userId)
        {
            var user = await _userRepo.GetById(userId);
            if (user != null)
            {
                user.IsAdmin = true;
                await _userRepo.Update(user);
            }
        }

        // Method to delete a user
        public async Task DeleteUser(int userId)
        {
            var user = await _userRepo.GetById(userId);
            await _userRepo.Delete(user);
        }
    }
}
