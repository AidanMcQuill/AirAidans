using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;//Added for access to required, display, etc...
using System.Data.SqlTypes;

namespace AirAidans.DATA.EF.Models
{
    #region Category

    public class CategoryMetadata
    {
        //since this a PK, we should never see it in a view, or have to enter a value when creating/editing.
        //For those reasons, we will not need to apply any metadata to a PK
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "* Category Name is required")]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }
    }

    #endregion

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
        //[StringLength(4, ErrorMessage = "Size Template: 9 | 12.5")]
        //[Display(Name = "Size")]
        public float size { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Color")]
        public string color { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "SKU")]
        public string SKU { get; set; } //Null

        //FK
        public int CategoryId { get; set; }
        //FK
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

    #region Supplier

    public class SupplierMetadata
    {
        public int SupplierId { get; set; }
        [Required]
        [StringLength(150)]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; } = null!;


        [Display(Name = "Town")]
        [StringLength(50)]
        public string? Town { get; set; } = null!;


        [Display(Name = "Country")]
        [StringLength(50)]
        public string? Country { get; set; }
 
    }
    #endregion

    #region UserDetails

    public class UserDetailMetadata
    {
        //PK
        public int UserId { get; set; }


        [StringLength(10)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(10)]
        [Display(Name = "Last Name")]
        
        public string? LastName { get; set; } = null!;

        [StringLength(10)]
        [Display(Name = "Country")]
        public string? Country { get; set; }
        [StringLength(50)]
       public string LockerId { get; set; } = null!;
    }

    #endregion
}
