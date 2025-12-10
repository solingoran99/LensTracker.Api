using Microsoft.AspNetCore.Mvc;
using LensTracker.Api.Models;
using LensTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LensTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LensController : ControllerBase
    {
        private readonly LensDbContext _db;

        public LensController(LensDbContext db)
        {
            _db = db;
        }

        // POST: api/lens/start
        [HttpPost("start")]
        public IActionResult StartLens([FromBody] StartLensRequest request)
        {
            int duration = request.Type.ToLower() switch
            {
                "daily" => 1,
                "biweekly" => 14,
                "monthly" => 30,
                _ => 30
            };

            // Stop previous session if exists
            var activeSession = _db.LensSessions.FirstOrDefault(s => s.IsActive);
            if (activeSession != null)
            {
                activeSession.IsActive = false;
                activeSession.StoppedEarlyDate ??= DateTime.UtcNow;
                _db.SaveChanges();
            }

            // Create new session
            var newSession = new LensSession
            {
                StartDate = request.StartDate,
                DurationDays = duration,
                EndDate = request.StartDate.AddDays(duration),
                IsActive = true
            };

            _db.LensSessions.Add(newSession);
            _db.SaveChanges();

            return Ok(new
            {
                message = "New lens session started.",
                newSession.StartDate,
                newSession.EndDate,
                newSession.DurationDays
            });
        }

        // GET: api/lens/status
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var session = _db.LensSessions
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault();

            if (session == null)
                return NotFound("No active lens session.");

            // Auto-expire
            if (DateTime.UtcNow.Date > session.EndDate.Date)
            {
                session.IsActive = false;
                _db.SaveChanges();

                return Ok(new
                {
                    session.StartDate,
                    session.EndDate,
                    session.DurationDays,
                    daysPassed = session.DurationDays,
                    daysLeft = 0,
                    isExpired = true
                });
            }

            var daysPassed = (DateTime.UtcNow.Date - session.StartDate.Date).Days;
            var daysLeft = (session.EndDate.Date - DateTime.UtcNow.Date).Days;

            return Ok(new
            {
                session.StartDate,
                session.EndDate,
                session.DurationDays,
                daysPassed = daysPassed < 0 ? 0 : daysPassed,
                daysLeft = daysLeft < 0 ? 0 : daysLeft,
                isExpired = false
            });
        }

        // POST: api/lens/stop
        [HttpPost("stop")]
        public IActionResult StopLens()
        {
            var session = _db.LensSessions.FirstOrDefault(s => s.IsActive);

            if (session == null)
                return NotFound("No active lens session to stop.");

            session.IsActive = false;
            session.StoppedEarlyDate = DateTime.UtcNow;
            _db.SaveChanges();

            return Ok(new
            {
                message = "Lens session stopped early.",
                session.StartDate,
                session.StoppedEarlyDate
            });
        }
    }
}
