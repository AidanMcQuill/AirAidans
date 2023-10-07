using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class UserPreference
    {
        public string UserId { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string? Color { get; set; }

        public virtual UserDetail User { get; set; } = null!;
    }
}
