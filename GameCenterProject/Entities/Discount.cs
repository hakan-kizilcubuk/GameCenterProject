using System;

namespace GameCenterProject.Entities
{
    public class Discount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameId { get; set; }
        public decimal Percentage { get; set; } // e.g. 0.15 for 15%
        public decimal? Amount { get; set; } // optional fixed amount off
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; } // admin user id or email
    }
}
