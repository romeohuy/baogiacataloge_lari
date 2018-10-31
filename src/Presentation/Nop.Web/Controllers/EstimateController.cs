using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Estimate;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Estimate;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Common;
using Nop.Web.Models.Customer;
using Nop.Web.Models.Estimate;

namespace Nop.Web.Controllers
{
    public partial class EstimateController : BasePublicController
    {
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
        private readonly IWorkContext _workContext;
        public EstimateController(ICustomerService customerService, ICustomerModelFactory customerModelFactory, IWorkContext workContext, IProductService productService, IEstimateInfoService estimateInfoService, IProductModelFactory productModelFactory, IShoppingCartService shoppingCartService, CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IStoreContext storeContext, IGenericAttributeService genericAttributeService)
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
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Index()
        {
            EstimatePriceModel model = new EstimatePriceModel();
            model.EstimateInfoModel.CreatedDate = DateTime.Now;

            PrepareCustomers(model);
            PrepareProductByTypeEstimateSteps(model);

            return View(model);
        }

        private void PrepareProductByTypeEstimateSteps(EstimatePriceModel model)
        {
            foreach (var typeEstimateStep in Enum.GetValues(typeof(TypeEstimateStep)).Cast<TypeEstimateStep>())
            {
                var products = _productService.GetProductsByTypeEstimateStep(typeEstimateStep);

                var modelStepModel = new ProductByTypeEstimateStepModel
                {
                    TypeEstimateStep = typeEstimateStep,
                    ProductOverviewModels = _productModelFactory.PrepareProductOverviewModels(products).ToList()
                };

                modelStepModel.ProductItems.Add(new SelectListItem {Value = "0", Text = "---"});
                modelStepModel.ProductItems.AddRange(products.Select(_=> new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                }));

                model.ProductByTypeEstimateSteps.Add(modelStepModel);
            
            }
        }

        private void PrepareCustomers(EstimatePriceModel model)
        {
            var customers = _customerService.GetAllCustomers();
            model.EstimateInfoModel.Customers.Add(new SelectListItem{ Value = "0",Text = "---"});
            foreach (var customer in customers)
            {
                var customerInfo = new CustomerInfoModel();
                customerInfo = _customerModelFactory.PrepareCustomerInfoModel(customerInfo, customer, false);
                if (!string.IsNullOrEmpty(customerInfo.GetFullName().Trim()))
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = customer.Id.ToString(),
                        Text = customerInfo.GetFullName()
                    };
                    model.EstimateInfoModel.Customers.Add(selectListItem);
                }
            }
        }

        [HttpGet]
        public IActionResult GetEstimateCustomerInfo(int customerId)
        {
            var estimateInfo = _estimateInfoService.GetUnFinishEstimateInfoByCustomerId(customerId);
            if (estimateInfo != null)
            {
                return Json(new JsonWebDataResult {Data = estimateInfo});
            }
            var customer = _customerService.GetCustomerById(customerId);
            var customerInfo = new CustomerInfoModel();
            _customerModelFactory.PrepareCustomerInfoModel(customerInfo, customer,false);
            var customerBasicInfo = new CustomerBasicInfoModel()
            {
                CustomerId = customerId,
                //Address = customer.Addresses.FirstOrDefault().Address1,
                CustomerFirstName = customerInfo.FirstName,
                CustomerLastName = customerInfo.LastName,
                CustomerPhone = customerInfo.Phone
            };
            return Json(new JsonWebDataResult() {Data = customerBasicInfo});
        }

        [HttpPost]
        public IActionResult EstimateInfoCreate(EstimateInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Username = model.CustomerPhone,
                    Email = model.CustomerPhone + "@youremail.com",
                    
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
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.FirstNameAttribute, model.CustomerFirstName);
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.LastNameAttribute, model.CustomerLastName);
                }
                var entity = model.ToEntity<EstimateInfo>();
                _estimateInfoService.InsertEstimateInfo(entity);
                if (entity.Id > 0)
                {
                    return Json(new {Success = true,EstimateInfoId = entity.Id});
                }
                else
                {
                    return Json(new {Success = false, Message = "Luu thong tin khong thanh cong."});
                }
            }
            else
            {
                return Json(new JsonWebDataResult {Success = false, Message = ""});
            }
        }

        [HttpPost]
        public IActionResult EstimateLoadProduct(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return Json(new JsonWebDataResult() {Success = false, Message = "Thông tin sản phẩm không hợp lệ."});
            return Json(new JsonWebDataResult { Success = true,Data = PartialView("_EstimateProductBiaInfo")});
        }


        //public IActionResult AddProductToEstimate(ProductOverviewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _shoppingCartService.AddToCart();
        //    }
        //}
    }
}