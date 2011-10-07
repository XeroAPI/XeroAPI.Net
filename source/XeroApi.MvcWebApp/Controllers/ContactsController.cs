using System.Linq;
using System.Web.Mvc;

using Xero.ScreencastWeb.Services;

namespace Xero.ScreencastWeb.Controllers
{
    [InitServiceProvider]
    public class ContactsController : ControllerBase
    {
        //
        // GET: /Contacts/

        public ActionResult Index()
        {
            var repository = ServiceProvider.GetCurrentRepository();

            return View(repository.Contacts.ToList());
        }

    }
}
