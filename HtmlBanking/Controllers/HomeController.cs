using System.Web.Mvc;

namespace HtmlBanking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Echo( MessageModel model )
        {
            var result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            //client.Interact(  )

            return result;
        }
    }

    public class MessageModel
    {
        public string Message { get; set; }
    }
}