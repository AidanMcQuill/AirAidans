using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class Locker
    {
        public int LockerId { get; set; }
        public string UserId { get; set; } = null!;
        public int ShoeId { get; set; }

        public virtual Shoe Shoe { get; set; } = null!;
        public virtual UserDetail User { get; set; } = null!;
    }
}
