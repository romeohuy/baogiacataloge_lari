using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Estimate;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Estimate;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Estimate;
using Nop.Web.Areas.Admin.Models.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class EstimateController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IEstimateInfoService _estimateInfoService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly IProductModelFactory _productModelFactory;
        private readonly CustomerSettings _customerSettings;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IPdfService _pdfService;

        public EstimateController(ICustomerService customerService, ICustomerModelFactory customerModelFactory, IWorkContext workContext, IProductService productService, IEstimateInfoService estimateInfoService, IProductModelFactory productModelFactory, IShoppingCartService shoppingCartService, CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IStoreContext storeContext, IGenericAttributeService genericAttributeService, ILocalizationService localizationService, IOrderService orderService, IPriceFormatter priceFormatter, IPdfService pdfService)
        {
            _customerService = customerService;
            _customerModelFactory = customerModelFactory;
            _workContext = workContext;
            _productService = productService;
            _estimateInfoService = estimateInfoService;
            _productModelFactory = productModelFactory;
            _shoppingCartService = shoppingCartService;
            _customerSettings = customerSettings;
            _customerRegistrationService = customerRegistrationService;
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _orderService = orderService;
            _priceFormatter = priceFormatter;
            _pdfService = pdfService;
        }

        public virtual IActionResult Index()
        {
            EstimatePriceModel model = new EstimatePriceModel();
            PrepareEstimateInfoNewModels(model);

            return View(model);
        }

        public virtual IActionResult Create(int id = 0, int typeEstimateStep = 0)
        {

            EstimatePriceModel model = new EstimatePriceModel();

            if (id > 0)
            {
                model.EstimateInfoModel = _estimateInfoService.GetEstimateInfoById(id).ToModel<EstimateInfoModel>();
            }
            else
            {
                model.EstimateInfoModel = new EstimateInfoModel { CreatedDate = DateTime.Now, TypeEstimateId = typeEstimateStep, ProfitPercent = 10 };
            }
            model.EstimateSummaryModel = new EstimateSummaryModel { TypeEstimateId = typeEstimateStep };
            PrepareCustomers(model);
            PrepareProductByTypeEstimateSteps(model);
            PrepareEstimateInfoNewModels(model);

            return View(model);
        }

        private void PrepareEstimateInfoNewModels(EstimatePriceModel model)
        {
            if (model != null)
            {
                model.EstimateInfoNewModels = _estimateInfoService.GetNewEstimateInfos().Select(_ => _.ToModel<EstimateInfoModel>()).ToList();
            }
        }

        private void PrepareProductByTypeEstimateSteps(EstimatePriceModel model)
        {
            foreach (var typeEstimateStep in Enum.GetValues(typeof(TypeEstimateStep)).Cast<TypeEstimateStep>())
            {
                var products = _productService.GetProductsByTypeEstimateStep(typeEstimateStep);

                var modelStepModel = new ProductByTypeEstimateStepModel
                {
                    TypeEstimateStep = typeEstimateStep,
                    ProductModels = products.Select(_ => _.ToModel<ProductModel>()).ToList()
                };

                modelStepModel.ProductItems.Add(new SelectListItem { Value = "0", Text = "---" });
                modelStepModel.ProductItems.AddRange(products.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                }));

                for (int i = 1; i < 10; i++)
                {
                    modelStepModel.HandPrints.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }

                model.ProductByTypeEstimateSteps.Add(modelStepModel);
            }
        }

        private void PrepareCustomers(EstimatePriceModel model)
        {
            var customers = _customerService.GetAllCustomers();
            model.EstimateInfoModel.Customers.Add(new SelectListItem { Value = "0", Text = "---" });
            foreach (var customer in customers)
            {
                var customerModel = _customerModelFactory.PrepareCustomerModel(null, customer);
                customerModel.FullName = _customerService.GetCustomerFullName(customer);

                if (!string.IsNullOrEmpty(customerModel.FullName))
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = customer.Id.ToString(),
                        Text = customerModel.FullName,
                        Selected = model.EstimateInfoModel.CustomerId == customer.Id
                    };
                    model.EstimateInfoModel.Customers.Add(selectListItem);
                }
            }
        }

        [HttpPost]
        public IActionResult ListShoppingCartItems(int typeEstimateStepId = 0, int customerId = 0, int estimateInfoId = 0)
        {
            IList<ShoppingCartItem> shoppingCartItems = new List<ShoppingCartItem>();
            if (typeEstimateStepId > 0 && customerId > 0 && estimateInfoId > 0)
            {
                shoppingCartItems = _shoppingCartService.GetShoppingCartItems(estimateInfoId, customerId, typeEstimateStepId);
            }
            var model = new ShoppingCartItemListModel()
            {
                Data = shoppingCartItems.Select(_ =>
                {
                    var shoppingCartItemModel = _.ToModel<ShoppingCartItemModel>();
                    shoppingCartItemModel.ProductName = _.Product.Name;
                    shoppingCartItemModel.UnitName = _.UnitName;
                    shoppingCartItemModel.ShortDescription = _.Product.ShortDescription;
                    shoppingCartItemModel.UnitPrice = _priceFormatter.FormatPrice(_.CustomerEnteredPrice, true, false);
                    if (_.Product.TypeProductPrint == TypeProductPrint.TinhTheoDonVi)
                    {
                        shoppingCartItemModel.Total = _priceFormatter.FormatPrice(_.Quantity * _.CustomerEnteredPrice, true, false);
                    }
                    else if (_.Product.TypeProductPrint == TypeProductPrint.TinhTheoDienTichMat)
                    {
                        shoppingCartItemModel.Total = _priceFormatter.FormatPrice(_.Length * _.Width * _.Quantity * _.CustomerEnteredPrice, true, false);
                    }
                    return shoppingCartItemModel;
                }),
                Total = shoppingCartItems.Count
            };
            return Json(model);
        }

        [HttpGet]
        public IActionResult GetEstimateCustomerInfo(int customerId)
        {
            var estimateInfo = _estimateInfoService.GetUnFinishEstimateInfoByCustomerId(customerId);
            if (estimateInfo != null)
            {
                return Json(new JsonWebDataResult { Data = estimateInfo });
            }
            var customer = _customerService.GetCustomerById(customerId);
            var customerInfo = _customerModelFactory.PrepareCustomerModel(null, customer);

            return Json(new JsonWebDataResult()
            {
                Data = new CustomerModel.CustomerBasicModel
                {
                    Id = customerId,
                    FirstName = customerInfo.FirstName,
                    LastName = customerInfo.LastName,
                    Phone = customerInfo.Phone
                }
            });
        }

        [HttpPost]
        public IActionResult EstimateInfoCreate(EstimateInfoModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer;
                if (model.CustomerId == 0)
                {
                    customer = new Customer
                    {
                        Username = model.CustomerPhone,
                        Email = model.CustomerPhone + "@youremail.com"
                    };
                    var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                    var registrationRequest = new CustomerRegistrationRequest(customer,
                        customer.Email,
                        model.CustomerPhone,
                        model.CustomerPhone, //password is same phone
                        _customerSettings.DefaultPasswordFormat,
                        _storeContext.CurrentStore.Id,
                        isApproved);
                    var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                    if (registrationResult.Success)
                    {
                        model.CustomerId = customer.Id;
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.FirstNameAttribute, model.CustomerFirstName);
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.LastNameAttribute, model.CustomerLastName);
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.PhoneAttribute, model.CustomerPhone);
                    }
                }
                else
                {
                    customer = _customerService.GetCustomerById(model.CustomerId);
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.FirstNameAttribute, model.CustomerFirstName);
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.LastNameAttribute, model.CustomerLastName);
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.PhoneAttribute, model.CustomerPhone);
                }
                if (customer.Id > 0)
                {
                    var entity = model.ToEntity<EstimateInfo>();
                    if (entity.Id == 0)
                    {
                        _estimateInfoService.InsertEstimateInfo(entity);
                        if (entity.Id > 0)
                        {
                            return Json(new { Success = true, EstimateInfoId = entity.Id });
                        }
                    }
                    else
                    {
                        _estimateInfoService.UpdateEstimateInfo(entity);
                        return Json(new { Success = true, EstimateInfoId = entity.Id });
                    }
                }
                return Json(new { Success = false, Message = _localizationService.GetResource("Admin.Estimate.Estimates.CreateFail") });
            }

            return Json(new JsonWebDataResult { Success = false, Message = _localizationService.GetResource("Admin.Commons.DataInvalid") });
        }

        [HttpPost]
        public IActionResult EstimateLoadProduct(int productId, int estimateId, int typeEstimateStepId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return Json(new JsonWebDataResult() { Success = false, Message = _localizationService.GetResource("Admin.Commons.ProductInfoDataInvalid") });

            var model = product.ToModel<ProductEstimateModel>();
            model.EstimateId = estimateId;
            model.ProductId = productId;
            model.CustomerEnteredPrice = product.Price;

            if (typeEstimateStepId == (int)TypeEstimateStep.Bia)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductBiaInfo", model) });
            }

            if (typeEstimateStepId == (int)TypeEstimateStep.BiaTruocIn)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductBiaTruocInInfo", model) });
            }

            if (typeEstimateStepId == (int)TypeEstimateStep.Ruot)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductRuotInfo", model) });
            }

            if (typeEstimateStepId == (int)TypeEstimateStep.RuotTruocIn)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductRuotTruocInInfo", model) });
            }

            if (typeEstimateStepId == (int)TypeEstimateStep.ThanhPham)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductThanhPhamInfo", model) });
            }
            return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductInfo", model) });
        }

        [HttpPost]
        public IActionResult AddOrUpdateProductToEstimate(ProductEstimateModel model, int customerId = 0)
        {
            var customer = _customerService.GetCustomerById(customerId);

            var product = _productService.GetProductById(model.ProductId);
            var shoppingCartItem = new ShoppingCartItem
            {
                Id = model.ShoppingCartItemId,
                EstimateId = model.EstimateId,
                CustomerId = customerId,
                Length = model.Length,
                Width = model.Width,
                Quantity = model.Quantity,
                ProductId = product.Id,
                ShoppingCartType = ShoppingCartType.Estimate,
                StoreId = _storeContext.CurrentStore.Id,
                TypeEstimateStepId = model.TypeEstimateStepId,
                CustomerEnteredPrice = model.CustomerEnteredPrice,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                UnitName = model.UnitName,
                HandPrint = model.HandPrint
            };
            if (model.ShoppingCartItemId == 0)
            {
                _shoppingCartService.InsertShoppingCartItem(shoppingCartItem);
            }
            else
            {
                _shoppingCartService.UpdateShoppingCartItem(shoppingCartItem);
            }
            return Json(new JsonWebDataResult { Success = true });
        }

        public IActionResult GetShoppingCartItem(int id)
        {
            var shoppingCartItem = _shoppingCartService.GetShoppingCartItemById(id);

            var product = _productService.GetProductById(shoppingCartItem.ProductId);
            if (product == null) return Json(new JsonWebDataResult() { Success = false, Message = _localizationService.GetResource("Admin.Commons.ProductInfoDataInvalid") });

            var model = product.ToModel<ProductEstimateModel>();
            if (shoppingCartItem.EstimateId != null) model.EstimateId = shoppingCartItem.EstimateId.Value;
            model.ProductId = product.Id;
            model.CustomerEnteredPrice = product.Price;
            model.Width = shoppingCartItem.Width;
            model.Length = shoppingCartItem.Length;
            model.CustomerEnteredPrice = shoppingCartItem.CustomerEnteredPrice;
            model.Quantity = shoppingCartItem.Quantity;
            model.UnitName = shoppingCartItem.UnitName;
            model.ShoppingCartItemId = shoppingCartItem.Id;
            model.TypeEstimateStep = shoppingCartItem.TypeEstimateStep;
            model.TypeEstimateStepId = shoppingCartItem.TypeEstimateStepId;
            if (shoppingCartItem.TypeEstimateStep == TypeEstimateStep.Bia)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductBiaInfo", model) });
            }

            if (shoppingCartItem.TypeEstimateStep == TypeEstimateStep.BiaTruocIn)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductBiaTruocInInfo", model) });
            }

            if (shoppingCartItem.TypeEstimateStep == TypeEstimateStep.Ruot)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductRuotInfo", model) });
            }

            if (shoppingCartItem.TypeEstimateStep == TypeEstimateStep.RuotTruocIn)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductRuotTruocInInfo", model) });
            }

            if (shoppingCartItem.TypeEstimateStep == TypeEstimateStep.ThanhPham)
            {
                return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductThanhPhamInfo", model) });
            }

            return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductInfo", model) });
        }

        [HttpPost]
        public IActionResult CalculateEstimateInfo(int id)
        {
            var estimateInfo = _estimateInfoService.GetEstimateInfoById(id);
            var model = new EstimateSummaryModel();
            if (estimateInfo != null)
            {

                var shoppingCartItemModels = _shoppingCartService.GetShoppingCartItems(id, customerId: 0, typeEstimateStepId: 0).Select(_ =>
                {
                    var shoppingCartItemModel = _.ToModel<ShoppingCartItemModel>();
                    //shoppingCartItemModel.ProductName = _.Product.Name;
                    //shoppingCartItemModel.UnitName = _.UnitName;
                    //shoppingCartItemModel.ShortDescription = _.Product.ShortDescription;
                    //shoppingCartItemModel.UnitPrice = _priceFormatter.FormatPrice(_.CustomerEnteredPrice, true, false);
                    if (_.Product.TypeProductPrint == TypeProductPrint.TinhTheoDonVi)
                    {
                        shoppingCartItemModel.Total = _priceFormatter.FormatPrice(_.Quantity * _.CustomerEnteredPrice, true, false);
                        shoppingCartItemModel.TotalDec = _.Quantity * _.CustomerEnteredPrice;
                    }
                    else if (_.Product.TypeProductPrint == TypeProductPrint.TinhTheoDienTichMat)
                    {
                        shoppingCartItemModel.Total = _priceFormatter.FormatPrice(_.Length * _.Width * _.Quantity * _.CustomerEnteredPrice, true, false);
                        shoppingCartItemModel.TotalDec = _.Length * _.Width * _.Quantity * _.CustomerEnteredPrice;
                    }
                    return shoppingCartItemModel;
                });
                var cartItemModels = shoppingCartItemModels as ShoppingCartItemModel[] ?? shoppingCartItemModels.ToArray();
                var tienGiaCong = cartItemModels.Where(_ => _.TypeEstimateStep == TypeEstimateStep.ThanhPham).Sum(s => s.TotalDec);
                var tienBia = cartItemModels.Where(_ => _.TypeEstimateStep == TypeEstimateStep.BiaTruocIn).Sum(s => s.TotalDec);
                var tienRuot = cartItemModels.Where(_ => _.TypeEstimateStep == TypeEstimateStep.RuotTruocIn).Sum(s => s.TotalDec);
                var tienHang = tienGiaCong + tienBia + tienRuot;
                var tienLoiNhuan = tienHang * estimateInfo.ProfitPercent / 100;
                var tongTien = tienLoiNhuan + tienHang;
                var donGiaSanPham = tongTien / estimateInfo.TotalNumber;

                estimateInfo.TotalEstimate = tongTien;
                estimateInfo.TotalWithoutProfit = tienHang;
                estimateInfo.TotalProfit = tienLoiNhuan;
                estimateInfo.UnitPriceProductItem = donGiaSanPham;

                _estimateInfoService.UpdateEstimateInfo(estimateInfo);

                model.LoiNhuan = estimateInfo.ProfitPercent.ToString(CultureInfo.InvariantCulture);
                model.DonGiaSanPham = _priceFormatter.FormatPrice(donGiaSanPham, true, false);
                model.TienLoiNhuan = _priceFormatter.FormatPrice(tienLoiNhuan, true, false);
                model.TienBia = _priceFormatter.FormatPrice(tienBia, true, false);
                model.TienRuot = _priceFormatter.FormatPrice(tienRuot, true, false);
                model.TongTienGiaCong = _priceFormatter.FormatPrice(tienGiaCong, true, false);
                model.DonGiaSanPham = _priceFormatter.FormatPrice(donGiaSanPham, true, false);
                model.TongDonHang = _priceFormatter.FormatPrice(tongTien, true, false);
            }

            return Json(new JsonWebDataResult() { Success = true, Data = model });
        }

        [HttpPost]
        public IActionResult ExportPdfEstimate(int selectedId)
        {
            var estimateInfo = _estimateInfoService.GetEstimateInfoById(selectedId);
            var shoppingCartItems = _shoppingCartService.GetShoppingCartItems(selectedId, customerId: 0, typeEstimateStepId: 0);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintEstimateToPdf(stream, estimateInfo, shoppingCartItems, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }

            return File(bytes, MimeTypes.ApplicationPdf, $"estimate_{estimateInfo.Id}_{DateTime.Now:dd_MM_yyyy}_{estimateInfo.NumberEstimate}.pdf");
        }

        public IActionResult DeleteShoppingCartItem(int id)
        {
            var shoppingCartItem = _shoppingCartService.GetShoppingCartItemById(id);
            if (shoppingCartItem != null)
            {
                _shoppingCartService.DeleteShoppingCartItem(shoppingCartItem);
            }

            return Json(new JsonWebDataResult { Success = true });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entity = _estimateInfoService.GetEstimateInfoById(id);
            if (entity != null)
            {
                _estimateInfoService.DeleteEstimateInfo(entity);
            }

            return RedirectToAction("Index");
        }
    }
}