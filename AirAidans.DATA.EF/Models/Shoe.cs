using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class Shoe
    {
        public Shoe()
        {
            Lockers = new HashSet<Locker>();
        }

        public int ShoeId { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public double Size { get; set; }
        public string Color { get; set; } = null!;
        public string? Sku { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string? ShoeDescription { get; set; }
        public string? ShoeImage { get; set; }
        public decimal? Price { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<Locker> Lockers { get; set; }
    }
}
