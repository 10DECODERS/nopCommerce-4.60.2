using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports
{

    /// <summary>
    /// Represents a low stock product search model
    /// </summary>
    public partial record WareHouseProductListModel : BaseSearchModel
    {
        #region Ctor

   

        #endregion

        #region Properties

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        
        public int WareHouseId { get; set; }

        public string WareHouseName { get; set; }

        public  string SKU { get; set; }

        public int StocKQuantity { get; set; }
        #endregion
    }
}
