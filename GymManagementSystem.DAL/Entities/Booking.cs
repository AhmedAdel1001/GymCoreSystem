using System;

namespace GymManagementSystem.DAL.Entities
{
    public class Booking : BaseEntity
    {
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.Now;
        public bool IsAttended { get; set; } = false;
    }
}

