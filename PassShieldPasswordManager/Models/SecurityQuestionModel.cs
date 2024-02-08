using System.ComponentModel.DataAnnotations;

namespace PassShieldPasswordManager.Models;

public class SecurityQuestionModel
{
    [Key]
    public int SecurityQuestionId { get; set; }
    public string Question { get; set; }
}