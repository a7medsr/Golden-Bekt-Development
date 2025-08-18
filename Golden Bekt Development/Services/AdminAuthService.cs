using Golden_Bekt_Development.Models;
using Golden_Bekt_Development.Models.Context;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Golden_Bekt_Development.Services
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly IConfiguration _config;
        private readonly GoldenDbContext _goldenDbContext;

        public AdminAuthService(IConfiguration config,GoldenDbContext goldenDbContext)
        {
            _config = config;
            _goldenDbContext = goldenDbContext;
        }

        public async Task<Admin> RegisterAsync(string username, string password, string name)
        {
            if (_goldenDbContext.Admins.Any(a => a.UserName == username))
                throw new Exception("Admin already exists");

            CreatePasswordHash(password, out string hash, out string salt);

            var admin = new Admin
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                UserName = username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _goldenDbContext.Admins.Add(admin);
            _goldenDbContext.SaveChanges();
            return await Task.FromResult(admin);
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var admin = _goldenDbContext.Admins.SingleOrDefault(a => a.UserName == username);
            if (admin == null) return null;

            if (!VerifyPasswordHash(password, admin.PasswordHash, admin.PasswordSalt))
                return null;

            return await Task.FromResult(GenerateToken(admin));
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new HMACSHA512();
            var saltBytes = hmac.Key;
            salt = Convert.ToBase64String(saltBytes);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            hash = Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == storedHash;
        }

        private string GenerateToken(Admin admin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id),
                new Claim(ClaimTypes.Name, admin.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}