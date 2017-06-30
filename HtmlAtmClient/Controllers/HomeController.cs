
using System.Web.Mvc;
using Bank;
using Grpc.Core;

namespace HtmlAtmClient.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    
    [HttpGet]
    public JsonResult Echo( string input )
    {

      var channel = new Channel( "127.0.0.1:50051", ChannelCredentials.Insecure );
      var client = new Bank.Bank.BankClient( channel );
      var reply = client.SayHello( new HelloRequest {Name = input} );
      var result = new JsonResult
      {
        Data = Json( reply.Message ),

        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
      };

      //client.SayHello(  )

      return result;
    }
  }
}
