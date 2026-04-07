using System;

namespace PasswordGenerator
{
    public class PasswordGeneratorEngine
    {
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        private readonly Random _random = new Random();

        public string GeneratePassword(int length, bool includeLowercase, bool includeUppercase, 
                                      bool includeDigits, bool includeSpecial)
        {
            if (length <= 0)
                throw new ArgumentException("Délka hesla musí být vìtší než 0.");

            string characterSet = BuildCharacterSet(includeLowercase, includeUppercase, 
                                                   includeDigits, includeSpecial);

            if (string.IsNullOrEmpty(characterSet))
                throw new ArgumentException("Musíte vybrat alespoò jednu možnost.");

            return GenerateRandomPassword(characterSet, length);
        }

        private string BuildCharacterSet(bool includeLowercase, bool includeUppercase, 
                                        bool includeDigits, bool includeSpecial)
        {
            string characterSet = string.Empty;

            if (includeLowercase)
                characterSet += LowercaseLetters;
            if (includeUppercase)
                characterSet += UppercaseLetters;
            if (includeDigits)
                characterSet += Digits;
            if (includeSpecial)
                characterSet += SpecialCharacters;

            return characterSet;
        }

        private string GenerateRandomPassword(string characterSet, int length)
        {
            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = characterSet[_random.Next(characterSet.Length)];
            }

            return new string(password);
        }
    }
}