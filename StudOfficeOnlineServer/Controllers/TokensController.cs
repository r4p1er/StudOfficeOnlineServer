using Microsoft.AspNetCore.Mvc;
using StudOfficeOnlineServer.Models;

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


    }
}
