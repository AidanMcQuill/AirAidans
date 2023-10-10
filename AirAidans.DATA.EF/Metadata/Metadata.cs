using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;//Added for access to required, display, etc...

namespace AirAidans.DATA.EF.Models
{
    #region Shoes
    public class ShoeMetadata
        //TODO come back and fill out the metadata to fix the site's list. 
    {
        public int Shoe_ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Brand")]
        public string Brand { get; set; } = null!;


        [Required]
        [StringLength(255)]
        [Display(Name = "Model")]
        public string Model { get; set; } = null!;


        [Required]
        [StringLength(4, ErrorMessage = "Size Template: 9 | 12.5")]
        [Display(Name = "Size")]
        public int size { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Color")]
        public string color { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "SKU")]
        public int SKU { get; set; } //Null

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        [StringLength(250)]
        [Display(Name = "Description")]
        public string ShoeDescription { get; set; } //Null
        
        [Display(Name = "Shoe Image")]
        public string ShoeImage { get; set; } //Null

        
        [Display(Name = "Shoe Price")]
        public decimal Price { get; set; } //Null
    }
    #endregion
}
