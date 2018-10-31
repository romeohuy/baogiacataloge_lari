using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        private readonly IWorkContext _workContext;

        public HomeController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Index()
        {
            if (_workContext.CurrentCustomer.IsGuest())
                return RedirectToAction("Login", "Customer");
            return RedirectToAction("Index", "Home", new { area = "Admin" });
            //return View();
        }
    }
}