using LibraryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers.User
{
    public class UserController : Controller
    {
        public IActionResult CreateUserInfo()
        {
            var model = new CreateUserModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateUserInfo(CreateUserModel model)
        {
            return View();
        }
    }
}
