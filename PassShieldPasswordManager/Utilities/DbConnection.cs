using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Utilities;

public class DbConnection
{
    private static DbConnection _instance;
    private readonly PassShieldContext _database;
    
    private DbConnection()
    {
        _database = new PassShieldContext();
    }

    public static DbConnection Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DbConnection();
            }
            return _instance;
        }
    }

    public PassShieldContext Database
    {
        get { return _database; }
    }
}