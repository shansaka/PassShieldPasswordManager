using System.Security.Cryptography;
using System.Text;

namespace PassShieldPasswordManager.Utilities
{
    // Encryption and decryption
    public class Encryption
    {
        // Properties
        private string Text { get; set; }  
        private string Key { get; set; }  
        private string Chars { get; set; }  
        
        // Constructor
        public Encryption(string text)
        {
            Text = text;
            Key = "_4i\"Qw,lUr6|:Hm%[;5ogM!u^~N3AFdO=v?R/L2G#(f-E\\c0DWjy@SP>`$Te*knBZ8{Jzt9q<}1xpXI7bYVKh]+.as)'&C";
            Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?/[]{},.;:'\"\\|`~";
        }

        // Method to encrypt the text
        public string Encrypt()
        {
            var encryptText = "";
            List<char> charList = Chars.ToList();
            foreach (char letter in Text)
            {
                // Get index of the letter in the character set
                int index = charList.IndexOf(letter);
                // Append corresponding character from the key
                encryptText += Key[index];
            }
            return encryptText;
        }
        
        // Method to decrypt the text
        public string Decrypt()
        {
            string decryptedText = "";
            List<char> charList = Chars.ToList();
            foreach (char letter in Text)
            {
                // Get index of the letter in the key
                int index = Key.IndexOf(letter);
                // Append corresponding character from the character set
                decryptedText += charList[index];
            }
            return decryptedText;
        }
        
        // Method to create SHA-512 hash
        public string CreateSha512()
        {
            var message = Encoding.UTF8.GetBytes(Text);
            using (var alg = SHA512.Create())
            {
                var hex = "";

                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }
    }
}
