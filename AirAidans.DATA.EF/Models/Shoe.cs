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
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public double Size { get; set; }
        public string Color { get; set; }  
        public string? Sku { get; set; }
        //FK
        public int CategoryId { get; set; }
        //FK
        public int SupplierId { get; set; } 
        public string? ShoeDescription { get; set; }
        public string? ShoeImage { get; set; }
        public decimal? Price { get; set; }

        public virtual Category? Category { get; set; }  
        public virtual Supplier? Supplier { get; set; } 
        public virtual ICollection<Locker> Lockers { get; set; }
    }
}
