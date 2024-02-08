namespace PassShieldPasswordManager.Models;
using Microsoft.EntityFrameworkCore;

public class PassShieldContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<SecurityQuestionModel> SecurityQuestions { get; set; }
    
    public DbSet<CredentialModel> Credentials { get; set; }

    public string DbPath { get; }

    public PassShieldContext()
    {
        DbPath = "pass_db.db";
    }

    // The following configures EF to use a Sqlite database file in the project folder.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
