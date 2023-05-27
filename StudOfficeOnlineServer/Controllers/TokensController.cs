using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly IConfiguration _configuration;

        public TokensController(DBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> GetToken(TokenDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            var randomNumber = new byte[32];
            string rt;

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                rt = Convert.ToBase64String(randomNumber);
            }

            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Password + _configuration["AuthOptions:PEPPER"], user.PasswordHash))
                {
                    return BadRequest(new { errors = "Password is incorrect." });
                }
            }
            else
            {
                user = await _db.Users.FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

                if (user == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                {
                    return BadRequest(new { errors = "Invalid credentials or refresh token." });
                }

                if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                {
                    return BadRequest(new { errors = "Refresh token has expired." });
                }
            }

            user.RefreshToken = rt;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            await _db.SaveChangesAsync();

            return new JsonResult(new { accessToken = WriteJWT(user), refreshToken = user.RefreshToken, role = user.Role, id = user.Id });
        }

        private string WriteJWT(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, Convert.ToString(user.Role)!)
            };
            var jwt = new JwtSecurityToken(
                issuer: _configuration["AuthOptions:ISSUER"],
                audience: _configuration["AuthOptions:AUDIENCE"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:KEY"]!)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
