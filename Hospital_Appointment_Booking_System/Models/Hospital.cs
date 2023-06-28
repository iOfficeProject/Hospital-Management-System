using System;
using System.Collections.Generic;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Hospital
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string TenantCode { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public int? UserId { get; set; }

        public virtual MasterUser? User { get; set; }
    }
}
