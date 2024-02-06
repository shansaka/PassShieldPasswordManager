using System.ComponentModel.DataAnnotations;

namespace PassShieldPasswordManager.Models;

public class Credentials
{
    [Key]
    public int CredentialId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Type { get; set; }
    public string Name { get; set; }
    public string UrlOrDeveloper { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }

    public Users User { get; set; }
    
}