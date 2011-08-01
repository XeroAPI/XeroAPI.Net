using System.Web.Mvc;
using Xero.ScreencastWeb.Models;
using Xero.ScreencastWeb.Services;

namespace Xero.ScreencastWeb.Controllers
{
    public class InvoicesController : ControllerBase
    {
        //
        // GET: /Invoices/

        public ActionResult Index()
        {
            ApiGetRequest<Invoice> listRequest = new ApiGetRequest<Invoice>
            {
                OrderByClause = "Date DESC",
                WhereClause = "AmountDue > 0"
            };

            ApiRepository repository = new ApiRepository();
            HttpSessionAccessTokenRepository accessTokenRepository = new HttpSessionAccessTokenRepository(Session);

            if (accessTokenRepository.GetToken("") == null)
            {
                return new ReturnToHomeResult("There is no access token for the current user. Please click the 'connect' button on the homepage.");
            }

            Response response = repository.Get(accessTokenRepository, listRequest);

            return View(response.Invoices);
        }

    }
}
