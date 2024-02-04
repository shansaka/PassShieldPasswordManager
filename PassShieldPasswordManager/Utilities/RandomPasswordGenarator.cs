namespace PassShieldPasswordManager.Utilities;

public class RandomPasswordGenerator
{
    public int Length { get; set; }
    public bool IncludeUppercase { get; set; }
    public bool IncludeDigits { get; set; }
    public bool IncludeSpecialChars { get; set; }

    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+";
    
    public string Generate()
    {
        var validChars = LowercaseChars;

        if (IncludeUppercase)
            validChars += UppercaseChars;

        if (IncludeDigits)
            validChars += Digits;

        if (IncludeSpecialChars)
            validChars += SpecialChars;
        
        var random = new Random();
        var chars = new char[Length];
        for (var i = 0; i < Length; i++)
        {
            chars[i] = validChars[random.Next(0, validChars.Length)];
        }

        return new string(chars);
    }
}