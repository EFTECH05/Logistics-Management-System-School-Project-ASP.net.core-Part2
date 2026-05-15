<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
=======
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GLMS.Controllers
>>>>>>> f91a0100db3c41b65957dbc13a57bed334be3bd4
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

<<<<<<< HEAD
        public IActionResult About()
        {
            return View();
        }

        // REPLACED FEATURES WITH CONTACT US
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Quote()
        {
            return View();
        }

        public IActionResult Services()
=======
        public IActionResult Privacy()
>>>>>>> f91a0100db3c41b65957dbc13a57bed334be3bd4
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
<<<<<<< HEAD
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
=======
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
>>>>>>> f91a0100db3c41b65957dbc13a57bed334be3bd4
