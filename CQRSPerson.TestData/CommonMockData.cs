using System;
using System.Linq;

namespace CQRSPerson.TestData
{
    public static class CommonMockData
    {
        private const string AlphabeticCharacters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
        private const string AlphaNumericCharacters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
        private static Random random = new Random();
        public static string GetRandomAlphabeticString(int length)
        {
            return new string(Enumerable.Repeat(AlphabeticCharacters, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetRandomAlphaNumericString(int length)
        {
            return new string(Enumerable.Repeat(AlphaNumericCharacters, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
