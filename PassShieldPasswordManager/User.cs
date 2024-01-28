using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager;

public class User : UserModel
{
    public int CreateCredentials()
    {
        return 0;
    }
}

public class Admin : User
{
    public bool DeleteUsers()
    {
        return true;
    }
}