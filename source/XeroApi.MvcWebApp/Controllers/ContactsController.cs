using System.Web.Mvc;
using System.Web.Routing;
using Xero.ScreencastWeb.Models;
using Xero.ScreencastWeb.Services;

namespace Xero.ScreencastWeb.Controllers
{
    public class ContactsController : ControllerBase
    {
        //
        // GET: /Contacts/

        public ActionResult Index()
        {
            var listRequest = new ApiGetRequest<Contact>
            {
                OrderByClause = "Name DESC",
                WhereClause = ""
            };

            ApiRepository repository = new ApiRepository();
            HttpSessionAccessTokenRepository accessTokenRepository = new HttpSessionAccessTokenRepository(Session);

            if (accessTokenRepository.GetToken("") == null)
            {
                return new ReturnToHomeResult("There is no access token for the current user. Please click the 'connect' button on the homepage.");
            }

            Response response = repository.Get(accessTokenRepository, listRequest);

            return View(response.Contacts);
        }

    }
}
