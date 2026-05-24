using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        // 🔐 ROLE PROTECTION
        var role = HttpContext.Session.GetString("Role");

        if (role != "Admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }
}