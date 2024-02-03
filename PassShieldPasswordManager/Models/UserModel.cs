using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassShieldPasswordManager.Models;

[Table("Users")]
public class UserModel
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int SecurityQuestionId { get; set; }
    public string SecurityAnswer { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsAdmin { get; set; }
}