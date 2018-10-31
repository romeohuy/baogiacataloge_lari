using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Estimate;
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
using System;
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
        private readonly IWorkContext _workContext;
        public EstimateController(ICustomerService customerService, ICustomerModelFactory customerModelFactory, IWorkContext workContext, IProductService productService, IEstimateInfoService estimateInfoService, IProductModelFactory productModelFactory, IShoppingCartService shoppingCartService, CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IStoreContext storeContext, IGenericAttributeService genericAttributeService, ILocalizationService localizationService)
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
                model.EstimateInfoModel = new EstimateInfoModel { CreatedDate = DateTime.Now, TypeEstimateId = typeEstimateStep };
            }
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
        public IActionResult EstimateLoadProduct(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return Json(new JsonWebDataResult() { Success = false, Message = _localizationService.GetResource("Admin.Commons.ProductInfoDataInvalid") });
            return Json(new JsonWebDataResult { Success = true, Data = RenderPartialViewToString("_EstimateProductBiaInfo", product.ToModel<ProductEstimateModel>()) });
        }

    }
}