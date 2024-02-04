using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PassShieldPasswordManager;

public class Encryption
{
    private string Text { get; set; }
    private string Key { get; set; }

    public Encryption(string text, string key = "")
    {
        Text = text;
        Key = key;
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