using PassShieldPasswordManager.Services;

namespace PassShieldPasswordManager.Utilities;
public class LoginSession
{
    private static LoginSession _instance;
    private User _user;
    private string _loggedInUsername;

    public static LoginSession Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoginSession();
            }
            return _instance;
        }
    }

    public User User
    {
        get { return _user; }
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(_loggedInUsername);
    }

    public string LoggedInUsername
    {
        get { return _loggedInUsername; }
    } 
    public void Login(User user)
    {
        File.WriteAllText("logged_in_user.txt", new Encryption(user.Username).Encrypt());
        if (user is Admin admin)
        {
            _user = admin;
        }
        else
        {
            _user = user;
        }
       
    }
    
    public void Logout()
    {
        File.Delete("logged_in_user.txt");
        _user = null;
    }
}