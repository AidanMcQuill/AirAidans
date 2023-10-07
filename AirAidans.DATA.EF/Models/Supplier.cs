using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Shoes = new HashSet<Shoe>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string? Town { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<Shoe> Shoes { get; set; }
    }
}
