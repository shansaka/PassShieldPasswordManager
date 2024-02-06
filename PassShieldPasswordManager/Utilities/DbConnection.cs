using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Utilities;

public class DbConnection
{
    private static DbConnection _instance;
    private readonly Context _database;
    
    private DbConnection()
    {
        _database = new Context();
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

    public Context Database
    {
        get { return _database; }
    }
}