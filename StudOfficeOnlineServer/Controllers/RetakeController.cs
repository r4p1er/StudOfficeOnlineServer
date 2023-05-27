using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers;

[ApiController]
[Route("api/retake")]
public class RetakeController : ControllerBase
{
    private readonly DBContext _ctx;
    private readonly IConfiguration _configuration;

    public RetakeController(DBContext ctx, IConfiguration configuration)
    {
        _ctx = ctx;
        _configuration = configuration;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<int> GetUserId()
    {
        var value = User.Claims.First(y => y.Type == ClaimTypes.Email).Value;
        var id = (await _ctx.Users
            .SingleOrDefaultAsync(x => x.Email == value)).Id;
        return id;
    }


    [HttpGet("groupsList"), Authorize("Teacher")]
    public async Task<IActionResult> GetGroupsList()
    {

        var userId = await GetUserId();

        var subjectsList = _ctx.Teachers.SingleOrDefaultAsync(x => x.Id == userId);

        return Ok(subjectsList);
    }

    [HttpPost, Authorize("Teacher")]
    public async Task<IActionResult> Create(RetakeCreationData retakeCreationData)
    {
        var retake = new Retake
        {
            Subject = retakeCreationData.Subject,
            TeacherId = await GetUserId(),
            GroupId = retakeCreationData.GroupId,
            DateTime = retakeCreationData.DateTime,
            Сlassroom = retakeCreationData.Сlassroom
        };

        await _ctx.Retake.AddAsync(retake);
        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(Create), retake);
    }

    [HttpGet("retakes"), Authorize("Students")]
    public async Task<IActionResult> GetRetakes()
    {
        var userId = await GetUserId();
        var user = await _ctx.Students.SingleOrDefaultAsync(x => x.Id == userId);
        var group = user.GroupId;

        var allRetakes = _ctx.Retake.ToList();
        var filteredRetakes = allRetakes.Where(x => x.GroupId == group);

        return Ok(filteredRetakes);
    }
}