using PassShieldPasswordManager.Services;

namespace PassShieldPasswordManager.Utilities;
public class LoginSession
{
    private static LoginSession _instance;
    private readonly Account _account;
    private User _user;
    
    private LoginSession()
    {
        _account = new Account();
        InitializeAsync().Wait();
    }
    
    private async Task InitializeAsync()
    {
        if (File.Exists("logged_in_user.txt"))
        {
            var username = await File.ReadAllTextAsync("logged_in_user.txt");
            var user = await _account.VerifyUsername(new Encryption(username).Decrypt());
            if (user is Admin admin)
            {
                _user = admin;
            }
            else
            {
                _user = user;
            }
        }
    }

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
        return _user != null;
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