using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Services;

namespace PassShieldPasswordManager.Utilities;

public class Mapper
{
    public Admin MapToUser(UserModel from, Admin to)
    {
        to.UserId = from.UserId;
        to.Name = from.Name;
        to.SecurityQuestionId = from.SecurityQuestionId;
        to.Password = from.Password;
        to.Username = from.Username;
        return to;
    }
    
    public User MapToUser(UserModel from, User to)
    {
        to.UserId = from.UserId;
        to.Name = from.Name;
        to.SecurityQuestionId = from.SecurityQuestionId;
        to.Password = from.Password;
        to.Username = from.Username;
        return to;
    }
    
    public UserModel MapToUserModel(User from, UserModel to)
    {
        to.UserId = from.UserId;
        to.Name = from.Name;
        to.SecurityQuestionId = from.SecurityQuestionId;
        to.SecurityAnswer = from.SecurityAnswer;
        to.Username = from.Username;
        to.Password = from.Password;
        return to;
    }
    
    public CredentialGame MapToCredential(CredentialModel from, CredentialGame to)
    {
        to.CredentialId = from.CredentialId;
        to.Password = from.Password;
        to.Username = from.Username;
        to.Developer = from.UrlOrDeveloper;
        to.GameName = from.Name;
        to.CreatedDate = from.CreatedDate;
        to.UpdatedDate = from.UpdatedDate;

        return to;
    }
    
    public CredentialWebsite MapToCredential(CredentialModel from, CredentialWebsite to)
    {
        to.CredentialId = from.CredentialId;
        to.Password = from.Password;
        to.Username = from.Username;
        to.Url = from.UrlOrDeveloper;
        to.WebsiteName = from.Name;
        to.CreatedDate = from.CreatedDate;
        to.UpdatedDate = from.UpdatedDate;

        return to;
    }
    
    public CredentialDesktopApp MapToCredential(CredentialModel from, CredentialDesktopApp to)
    {
        to.CredentialId = from.CredentialId;
        to.Password = from.Password;
        to.Username = from.Username;
        to.DesktopAppName = from.Name;
        to.CreatedDate = from.CreatedDate;
        to.UpdatedDate = from.UpdatedDate;

        return to;
    }
    
    
    public SecurityQuestion MapToSecurityQuestion(SecurityQuestionModel from, SecurityQuestion to)
    {
        to.SecurityQuestionId = from.SecurityQuestionId;
        to.Question = from.Question;
        return to;
    }
    
}