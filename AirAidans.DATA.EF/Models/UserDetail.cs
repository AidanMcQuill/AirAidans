using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class UserDetail
    {
        public UserDetail()
        {
            Lockers = new HashSet<Locker>();
        }

        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string LockerId { get; set; } = null!;

        
        public virtual ICollection<Locker> Lockers { get; set; }
    }
}
