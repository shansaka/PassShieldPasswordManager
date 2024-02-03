using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassShieldPasswordManager.Models;

[Table("SecurityQuestions")]
public class SecurityQuestionModel
{
    [Key]
    public int SecurityQuestionId { get; set; }
    public string Question { get; set; }
}