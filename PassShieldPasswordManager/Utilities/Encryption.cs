using System.Security.Cryptography;
using System.Text;

namespace PassShieldPasswordManager.Utilities;

public class Encryption
{
    private string Text { get; set; }
    private string Key { get; set; }
    private string Chars { get; set; }

    public Encryption(string text)
    {
        Text = text;
        Key = "_4i\"Qw,lUr6|:Hm%[;5ogM!u^~N3AFdO=v?R/L2G#(f-E\\c0DWjy@SP>`$Te*knBZ8{Jzt9q<}1xpXI7bYVKh]+.as)'&C";
        Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?/[]{},.;:'\"\\|`~";
    }

    public string Encrypt()
    {
        var encryptText = "";
        List<char> charList = Chars.ToList();
        foreach (char letter in Text)
        {
            int index = charList.IndexOf(letter);
            encryptText += Key[index];
        }

        return encryptText;
    }
    
    public string Decrypt()
    {
        string decryptedText = "";
        List<char> charList = Chars.ToList();
        foreach (char letter in Text)
        {
            int index = Key.IndexOf(letter);
            decryptedText += charList[index];
        }

        return decryptedText;
    }
    
    public string CreateSha512()
    {
        var message = Encoding.UTF8.GetBytes(Text);
        using (var alg = SHA512.Create())
        {
            string hex = "";

            var hashValue = alg.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}