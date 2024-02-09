namespace PassShieldPasswordManager.Utilities
{
    // Class for generating random passwords
    public class RandomPasswordGenerator
    {
        // Properties
        public int Length { get; set; } 
        public bool IncludeUppercase { get; set; } 
        public bool IncludeDigits { get; set; }
        public bool IncludeSpecialChars { get; set; } 

        // Constants for character sets
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_=+";

        // Method to generate a random password
        public string Generate()
        {
            // Valid characters for password generation
            var validChars = LowercaseChars;

            // Include uppercase characters if specified
            if (IncludeUppercase)
                validChars += UppercaseChars;

            // Include digits if specified
            if (IncludeDigits)
                validChars += Digits;

            // Include special characters if specified
            if (IncludeSpecialChars)
                validChars += SpecialChars;
            
            // Generate random password using valid characters
            var random = new Random();
            var chars = new char[Length];
            for (var i = 0; i < Length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }
    }
}