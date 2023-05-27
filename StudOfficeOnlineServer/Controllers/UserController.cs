using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DBContext _ctx;
        private readonly IConfiguration _configuration;
        
        public UserController(DBContext ctx, IConfiguration configuration)
        {
            _ctx = ctx;
            _configuration = configuration;
        }
        
        
        [HttpGet("list")]
        public async Task<IActionResult> GetUsersList()
        {
            var allUsers = await _ctx.Users.ToListAsync();

            return Ok(allUsers);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == id);

            return Ok(user);
        }
        
        [HttpPatch("{userPatchData})")]
        public async Task<IActionResult> Update(UserPatchData userPatchData)
        {
            var user = _ctx.Users.SingleOrDefault(x => x.Id == userPatchData.Id);

            if (string.IsNullOrEmpty(user.Email))
                user.Email = userPatchData.Email;
            
            if (string.IsNullOrEmpty(user.PasswordHash))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userPatchData.PasswordHash + _configuration["AuthOptions:PEPPER"]);
            
            if (user.Role == null)
                user.Role = userPatchData.Role;
            
            if (string.IsNullOrEmpty(user.FirstName))
                user.FirstName = userPatchData.FirstName;
            
            if (string.IsNullOrEmpty(user.MiddleName))
                user.MiddleName = userPatchData.MiddleName;
            
            if (string.IsNullOrEmpty(user.LastName))
                user.LastName = userPatchData.LastName;
            
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
            
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == id);
            
            if (user == null)
                return BadRequest("User cannot be found.");
            
            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(UserCreationData userCreationData)
        {
            var user = new User
            {
                Email = userCreationData.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(userCreationData.PasswordHash +
                                                   _configuration["AuthOptions:PEPPER"]),
                Role = userCreationData.Role,
                FirstName = userCreationData.FirstName,
                MiddleName = userCreationData.MiddleName,
                LastName = userCreationData.LastName
            };

            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), user);
        }
        
    }
}