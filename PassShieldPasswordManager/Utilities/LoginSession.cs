namespace PassShieldPasswordManager.Utilities;
public class LoginSession
{
    private static LoginSession _instance;
    private readonly Account _account = new();
    private User _user;
    
    private LoginSession()
    {
        InitializeAsync().Wait();
    }
    
    private async Task InitializeAsync()
    {
        if (File.Exists("loggedinuser.txt"))
        {
            var username = await File.ReadAllTextAsync("loggedinuser.txt");
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
        File.WriteAllText("loggedinuser.txt", new Encryption(user.Username).Encrypt());
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
        File.Delete("loggedinuser.txt");
        _user = null;
    }
}