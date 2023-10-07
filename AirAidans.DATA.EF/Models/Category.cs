using System;
using System.Collections.Generic;

namespace AirAidans.DATA.EF.Models
{
    public partial class Category
    {
        public Category()
        {
            Shoes = new HashSet<Shoe>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }

        public virtual ICollection<Shoe> Shoes { get; set; }
    }
}
