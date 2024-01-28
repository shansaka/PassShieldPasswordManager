using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using PassShieldPasswordManager.Models;

public class DbConnection
{
    private static DbConnection _instance;
    private SqliteConnection _db;
    
    private DbConnection()
    {
        string connectioString = "pass_db.db";
        _db = new SqliteConnection($"Data Source={connectioString}"); 
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

    public SqliteConnection Db
    {
        get { return _db; }
    }
}