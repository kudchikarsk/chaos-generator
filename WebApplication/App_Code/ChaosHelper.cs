using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class ChaosHelper {
    private static string PASSWORD_CHAR_UPPER = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private static string PASSWORD_CHAR_LOWER = "abcdefghijkmnopqrstuvwxyz";
    private static string PASSWORD_CHAR_NUMBERS = "23456789";
    private static string PASSWORD_CHARS_SPECIAL = "!@#$%&*()=-+.:;/";

    public enum KeyEncoding {
        DecArray, HexArray, UpperHex, LowerHex, Base64, UrlSafeBase64
    }

    [Flags]
    public enum PasswordChars {
        UpperCaseLetters = 1, LowerCaseLetters = 2, Numbers = 4, Special = 8,
        LettersAndNumbers = UpperCaseLetters | LowerCaseLetters | Numbers,
        All = UpperCaseLetters | LowerCaseLetters | Numbers | Special
    }

    public static string GenerateKey(int length, KeyEncoding encoding) {
        // Validate arguments
        if (length < 1) throw new ArgumentOutOfRangeException("length");

        // Get random bytes
        var chaos = new byte[length];
        using (var rng = new RNGCryptoServiceProvider()) {
            rng.GetBytes(chaos);
        }

        // Format random bytes to various formats
        return FormatByteArray(chaos, encoding);
    }

    public static string FormatByteArray(byte[] data, KeyEncoding encoding) { 
            switch (encoding) {
            case KeyEncoding.DecArray:
                return string.Join(", ", from b in data select b);
            case KeyEncoding.HexArray:
                return "0x" + string.Join(", 0x", from b in data select b.ToString("X2"));
            case KeyEncoding.UpperHex:
                return string.Join(string.Empty, from b in data select b.ToString("X2"));
            case KeyEncoding.LowerHex:
                return string.Join(string.Empty, from b in data select b.ToString("x2"));
            case KeyEncoding.Base64:
                return Convert.ToBase64String(data);
            case KeyEncoding.UrlSafeBase64:
                return Convert.ToBase64String(data).Replace('-', '+').Replace('/', '_');
            default:
                throw new ArgumentOutOfRangeException("encoding");
        }
    }

    public static string GeneratePassword(int length, PasswordChars allowedCharTypes) {
        // Validate arguments
        if (length < 1) throw new ArgumentOutOfRangeException("length");

        // Build list of allowed characters
        var allowedChars = string.Empty;
        if ((allowedCharTypes & PasswordChars.LowerCaseLetters) == PasswordChars.LowerCaseLetters) allowedChars += PASSWORD_CHAR_LOWER;
        if ((allowedCharTypes & PasswordChars.UpperCaseLetters) == PasswordChars.UpperCaseLetters) allowedChars += PASSWORD_CHAR_UPPER;
        if ((allowedCharTypes & PasswordChars.Numbers) == PasswordChars.Numbers) allowedChars += PASSWORD_CHAR_NUMBERS;
        if ((allowedCharTypes & PasswordChars.Special) == PasswordChars.Special) allowedChars += PASSWORD_CHARS_SPECIAL;
        if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentOutOfRangeException("allowedCharTypes");

        // Get random password
        var rnd = GetSeededRandom();
        var sb = new StringBuilder();
        for (int i = 0; i < length; i++) {
            sb.Append(allowedChars[rnd.Next(allowedChars.Length)]);
        }
        return sb.ToString();
    }

    // Helper method to get instance of Random class seeded by crypto RNG

    private static Random GetSeededRandom() {
        // Get random Int32 seed
        var bytes = new byte[4];
        using (var rng = new RNGCryptoServiceProvider()) {
            rng.GetBytes(bytes);
        }

        // Return seeded Random class
        return new Random(BitConverter.ToInt32(bytes, 0));
    }

}