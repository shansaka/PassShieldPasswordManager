using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager;

public class User 
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int SecurityQuestionId { get; set; }
    public string SecurityAnswer { get; set; }
    public DateTime DateCreated { get; set; }
    
    public int CreateCredentials()
    {
        return 0;
    }
}