using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers;

[ApiController]
[Route("api/announcement")]
public class AnnouncementController : ControllerBase
{
    private readonly DBContext _ctx;

    public AnnouncementController(DBContext ctx)
    {
        _ctx = ctx;
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<int> GetUserId()
    {
        var value = User.Claims.First(y => y.Type == ClaimTypes.Name).Value;
        var id = (await _ctx.Users
            .SingleOrDefaultAsync(x => x.Email == value)).Id;
        return id;
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(AnnouncementData announcementData)
    {
        var id = await GetUserId();
        
        var announcement = new Announcement
        {
            Title = announcementData.Title,
            Content =  announcementData.Content,
            Date = DateTime.UtcNow
        };

        if (announcementData.GroupId != 0)
            announcement.GroupId = announcementData.GroupId;
        
        if (announcementData.FacultyId != 0)
            announcement.FacultyId = announcementData.FacultyId;
        
        if (announcementData.Course != 0)
            announcement.Course = announcementData.Course;

        await _ctx.Announcemenments.AddAsync(announcement);
        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(Create), announcement);
    }

    [HttpGet("list"), Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetList()
    {
        var userId = await GetUserId();

        var user = await _ctx.Students.SingleOrDefaultAsync(x => x.Id == userId);
        var allAnnouncements = await _ctx.Announcemenments.ToListAsync();
        var suitableAnnouncements = allAnnouncements.Select(x => 
            x.FacultyId == user.FacultyId &&
            x.GroupId == user.GroupId);
        
        return Ok(suitableAnnouncements);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromQuery]AnnouncementData announcementData)
    {
        var announcement = await _ctx.Announcemenments.SingleOrDefaultAsync(x => x.Id == id);
            
        if (announcement == null)
            BadRequest("Announcement cannot be found.");

        if (!string.IsNullOrEmpty(announcementData.Content))
            announcement.Content = announcementData.Content;
        
        if (!string.IsNullOrEmpty(announcementData.Title))
            announcement.Title = announcementData.Title;
        
        if (announcementData.Date != new DateTime(0,0,0))
            announcement.Date = announcementData.Date;
        
        if (announcementData.FacultyId != 0)
            announcement.FacultyId = announcementData.FacultyId;
        
        if (announcementData.Course != 0)
            announcement.Course = announcementData.Course;
        
        if (announcementData.GroupId != 0)
            announcement.GroupId = announcementData.GroupId;

        return Ok(announcement);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var announcement = await _ctx.Announcemenments.SingleOrDefaultAsync(x => x.Id == id);

        if (announcement == null)
            BadRequest("Announcement cannot be found.");
            
        _ctx.Announcemenments.Remove(announcement);
        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}