using System;
using System.Security.Cryptography;
using System.Text;

namespace SC2BM.Core.Security
{
    public class TokenGenerator
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings

        public static string GenerateToken(string username, string ip, string userAgent, long ticks)
        {
            string hash = string.Join(":", username, ip, userAgent, ticks.ToString());
            string hashLeft = "";
            string hashRight = "";

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(ip));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", username, ticks.ToString());
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", password, _salt);

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));

                return Convert.ToBase64String(hmac.Hash);
            }
        }
    }
}