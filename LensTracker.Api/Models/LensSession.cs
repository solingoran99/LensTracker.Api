using System.ComponentModel.DataAnnotations;

namespace LensTracker.Api.Models
{
    public class LensSession
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public int DurationDays { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StoppedEarlyDate { get; set; }
    }
}
