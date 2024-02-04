using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassShieldPasswordManager.Models;

public class SecurityQuestions
{
    [Key]
    public int SecurityQuestionId { get; set; }
    public string Question { get; set; }
}