using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirAidans.DATA.EF.Models
{
    #region Category
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }
    #endregion

    #region Supplier
    [ModelMetadataType(typeof(SupplierMetadata))]
    public partial class Supplier { }
    #endregion

    #region UserDetail
    [ModelMetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
    #endregion

    #region Shoes
    [ModelMetadataType(typeof(ShoeMetadata))]
    public partial class Shoe
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
        #endregion
    }

}
