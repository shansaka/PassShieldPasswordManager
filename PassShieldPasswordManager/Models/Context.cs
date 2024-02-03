namespace PassShieldPasswordManager.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class Context : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<SecurityQuestionModel> SecurityQuestions { get; set; }

    public string DbPath { get; }

    public Context()
    {
        DbPath = "pass_db.db";
    }

    // The following configures EF to use a Sqlite database file in the project folder.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
