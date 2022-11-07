using Microsoft.AspNetCore.Mvc;

namespace AxolotlProject.Controllers
{
    public class BusinessCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
