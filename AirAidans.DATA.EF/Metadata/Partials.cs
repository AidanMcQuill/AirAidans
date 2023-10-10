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
    #region Shoes
    [ModelMetadataType(typeof(ShoeMetadata))]
    public partial class Shoe { }
    #endregion
}
