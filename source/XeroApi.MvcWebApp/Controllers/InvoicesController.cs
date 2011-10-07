using System.Linq;
using System.Web.Mvc;

using Xero.ScreencastWeb.Services;

namespace Xero.ScreencastWeb.Controllers
{
    [InitServiceProvider]
    public class InvoicesController : ControllerBase
    {
        //
        // GET: /Invoices/

        public ActionResult Index()
        {
            var repository = ServiceProvider.GetCurrentRepository();

            return View(repository.Invoices.ToList());
        }

    }
}
