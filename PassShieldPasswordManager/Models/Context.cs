namespace PassShieldPasswordManager.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class Context : DbContext
{
    public DbSet<Users> Users { get; set; }
    public DbSet<SecurityQuestions> SecurityQuestions { get; set; }
    
    public DbSet<Credentials> Credentials { get; set; }

    public string DbPath { get; }

    public Context()
    {
        DbPath = "pass_db.db";
    }

    // The following configures EF to use a Sqlite database file in the project folder.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
