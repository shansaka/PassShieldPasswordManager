using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Services;

namespace PassShieldPasswordManager.Utilities
{
    // Class for mapping between different object types
    public class Mapper
    {
        // Method to map UserModel to Admin object
        public Admin MapToUser(UserModel from, Admin to)
        {
            // Mapping properties
            to.UserId = from.UserId;
            to.Name = from.Name;
            to.SecurityQuestionId = from.SecurityQuestionId;
            to.Password = from.Password;
            to.Username = from.Username;
            return to;
        }
        
        // Method to map UserModel to User object
        public User MapToUser(UserModel from, User to)
        {
            // Mapping properties
            to.UserId = from.UserId;
            to.Name = from.Name;
            to.SecurityQuestionId = from.SecurityQuestionId;
            to.Password = from.Password;
            to.Username = from.Username;
            return to;
        }
        
        // Method to map User to UserModel object
        public UserModel MapToUserModel(User from, UserModel to)
        {
            // Mapping properties
            to.UserId = from.UserId;
            to.Name = from.Name;
            to.SecurityQuestionId = from.SecurityQuestionId;
            to.SecurityAnswer = from.SecurityAnswer;
            to.Username = from.Username;
            to.Password = from.Password;
            return to;
        }
        
        // Method to map CredentialModel to CredentialGame object
        public CredentialGame MapToCredential(CredentialModel from, CredentialGame to)
        {
            // Mapping properties
            to.CredentialId = from.CredentialId;
            to.Password = from.Password;
            to.Username = from.Username;
            to.Developer = from.UrlOrDeveloper;
            to.GameName = from.Name;
            to.CreatedDate = from.CreatedDate;
            to.UpdatedDate = from.UpdatedDate;

            return to;
        }
        
        // Method to map CredentialModel to CredentialWebsite object
        public CredentialWebsite MapToCredential(CredentialModel from, CredentialWebsite to)
        {
            // Mapping properties
            to.CredentialId = from.CredentialId;
            to.Password = from.Password;
            to.Username = from.Username;
            to.Url = from.UrlOrDeveloper;
            to.WebsiteName = from.Name;
            to.CreatedDate = from.CreatedDate;
            to.UpdatedDate = from.UpdatedDate;

            return to;
        }
        
        // Method to map CredentialModel to CredentialDesktopApp object
        public CredentialDesktopApp MapToCredential(CredentialModel from, CredentialDesktopApp to)
        {
            // Mapping properties
            to.CredentialId = from.CredentialId;
            to.Password = from.Password;
            to.Username = from.Username;
            to.DesktopAppName = from.Name;
            to.CreatedDate = from.CreatedDate;
            to.UpdatedDate = from.UpdatedDate;

            return to;
        }
        
        
        // Method to map SecurityQuestionModel to SecurityQuestion object
        public SecurityQuestion MapToSecurityQuestion(SecurityQuestionModel from, SecurityQuestion to)
        {
            // Mapping properties
            to.SecurityQuestionId = from.SecurityQuestionId;
            to.Question = from.Question;
            return to;
        }
    }
}
