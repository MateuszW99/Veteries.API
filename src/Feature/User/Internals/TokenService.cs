using System;
using System.Security.Cryptography;
using User.Abstractions;

namespace User.Internals
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(int size)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}