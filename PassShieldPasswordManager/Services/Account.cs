using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services
{
    public class Account
    {
        // Dependencies
        private readonly IUserRepo _userRepo;
        private readonly ICredentialRepo _credentialRepo;
        private readonly Mapper _mapper = new(); // Utilized for mapping between domain and models
        private readonly LoginSession _loginSession = LoginSession.Instance; // Represents the current login session

        // Constructor injection for repositories
        public Account(IUserRepo userRepo, ICredentialRepo credentialRepo)
        {
            _userRepo = userRepo;
            _credentialRepo = credentialRepo;
        }
        
        // Method to login a user
        public async Task<User> Login(string username, string password)
        {
            try
            {
                // Authenticating and getting user information
                var users = await _userRepo.Login(username, password);
                if (users != null)
                {
                    // Check if the user is an admin
                    if (users.IsAdmin)
                    {
                        var admin = new Admin(_userRepo, _credentialRepo);
                        return _mapper.MapToUser(users, admin);
                    }
                    else
                    {
                        var user = new User(_credentialRepo);
                        return _mapper.MapToUser(users, user);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to register a new user
        public async Task<User> Register(User user)
        {
            try
            {
                // Mapping user input to UserModel
                var users = _mapper.MapToUserModel(user, new UserModel());
                // Encrypting the password
                users.Password = new Encryption(users.Password).CreateSha512();
                // Saving the user
                return _mapper.MapToUser(await _userRepo.Add(users), user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Method to verify if a username exists
        public async Task<User> VerifyUsername(string username)
        {
            try
            {
                var users = await _userRepo.GetByUsername(username);
                if (users != null)
                {
                    // Check if the user is an admin
                    if (users.IsAdmin)
                    {
                        var admin = new Admin(_userRepo, _credentialRepo);
                        return _mapper.MapToUser(users, admin);
                    }
                    else
                    {
                        var user = new User(_credentialRepo);
                        return _mapper.MapToUser(users, user);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Method to verify the security answer of a user
        public async Task<bool> VerifySecurityAnswer(int userId, int securityQuestionId, string securityAnswer)
        {
            try
            {
                return await _userRepo.VerifySecurityAnswer(userId, securityQuestionId, securityAnswer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Method to reset a user's password
        public async Task ResetPassword(int userId, string newPassword)
        {
            try
            {
                var user = await _userRepo.GetById(userId);
                if (user != null)
                {
                    // Encrypting the new password
                    user.Password = new Encryption(newPassword).CreateSha512();
                    // Updating the user with the new password
                    await _userRepo.Update(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Method to logout a user
        public void Logout()
        {
            _loginSession.Logout();
        }
    }
}
