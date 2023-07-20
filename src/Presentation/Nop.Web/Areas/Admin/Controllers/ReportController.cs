using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Reports;
using Nop.Web.Areas.Admin.Models.Shipping;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class ReportController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IReportModelFactory _reportModelFactory;
        private readonly IShippingModelFactory _shippingModelFactory;
        #endregion

        #region Ctor

        public ReportController(
            IPermissionService permissionService,
            IShippingModelFactory shippingModelFactory,
            IReportModelFactory reportModelFactory)
        {
            _permissionService = permissionService;
            _reportModelFactory = reportModelFactory;
            _shippingModelFactory = shippingModelFactory;
        }

        #endregion

        #region Methods

        #region Sales summary

        public virtual async Task<IActionResult> SalesSummary()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.SalesSummaryReport))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareSalesSummarySearchModelAsync(new SalesSummarySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesSummaryList(SalesSummarySearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.SalesSummaryReport))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareSalesSummaryListModelAsync(searchModel);

            return Json(model);
        }


        #endregion

        #region Low stock

        public virtual async Task<IActionResult> LowStock()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareLowStockProductSearchModelAsync(new LowStockProductSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> LowStockList(LowStockProductSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareLowStockProductListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Daily Sales

        public virtual async Task<IActionResult> DailySales()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();
            
            //prepare model
            var model = await _reportModelFactory.PrepareDailySalesProductSearchModelAsync(new DailySalesProductSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DailySalesList(DailySalesProductSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareDailySalesProductListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Inventory

        public virtual async Task<IActionResult> Warehouses()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //prepare model
            var model = await _shippingModelFactory.PrepareWarehouseSearchModelAsync(new WarehouseSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Warehouses(WarehouseSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _shippingModelFactory.PrepareWarehouseListModelAsync(searchModel);

            return Json(model);
        }


        public virtual async Task<IActionResult> WarehouseProductsList(WarehouseSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareWarehouseProductsListModelAsync(searchModel);

            return View(model);
        }





        #endregion


        #region Bestsellers

        public virtual async Task<IActionResult> Bestsellers()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareBestsellerSearchModelAsync(new BestsellerSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> BestsellersList(BestsellerSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareBestsellerListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> BestsellersReportAggregates(BestsellerSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var totalAmount = await _reportModelFactory.GetBestsellerTotalAmountAsync(searchModel);

            return Json(new { aggregatortotal = totalAmount });
        }

        #endregion

        #region Never Sold

        public virtual async Task<IActionResult> NeverSold()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareNeverSoldSearchModelAsync(new NeverSoldReportSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> NeverSoldList(NeverSoldReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareNeverSoldListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Country sales

        public virtual async Task<IActionResult> CountrySales()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.OrderCountryReport))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareCountrySalesSearchModelAsync(new CountryReportSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CountrySalesList(CountryReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.OrderCountryReport))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareCountrySalesListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Customer reports

        public virtual async Task<IActionResult> RegisteredCustomers()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

            return View(model);
        }

        public virtual async Task<IActionResult> BestCustomersByOrderTotal()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

            return View(model);
        }

        public virtual async Task<IActionResult> BestCustomersByNumberOfOrders()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ReportBestCustomersByOrderTotalList(BestCustomersReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareBestCustomersReportListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ReportBestCustomersByNumberOfOrdersList(BestCustomersReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareBestCustomersReportListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ReportRegisteredCustomersList(RegisteredCustomersReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareRegisteredCustomersReportListModelAsync(searchModel);

            return Json(model);
        }        

        #endregion

        #endregion
    }
}
