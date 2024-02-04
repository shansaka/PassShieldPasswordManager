using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager.Utilities;

public class DbConnection
{
    private static DbConnection _instance;
    private Context _db;
    
    private DbConnection()
    {
        _db = new Context();
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

    public Context Db
    {
        get { return _db; }
    }
}