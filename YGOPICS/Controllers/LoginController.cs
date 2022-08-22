using Microsoft.AspNetCore.Mvc;
using YGOPICS.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YGOPICS.Models;
using YGOPICS.Models.ViewModels;

namespace YGOPICS.Controllers
{
    public class LoginController : Controller
    {

        private readonly ygopicsdbContext _context;

        public LoginController(ygopicsdbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public async Task <IActionResult> Users() 
        {
            return View(await _context.Users.ToListAsync());
        }

        

        [ValidateAntiForgeryToken]
        public IActionResult Enter(UserViewModel model)
        {
            
                try
                {
                    using (var db = new ygopicsdbContext())
                    {
                        var lst = from d in db.Users
                                  where d.Mail == model.Mail && d.Password == CypherHelper.CifrateHash(model.Password)
                                  select d;
                        if (lst.Count() > 0)
                        {
                            var userSession = lst.First();
                            HttpContext.Session.SetString("_Name", userSession.Name);
                            HttpContext.Session.SetInt32("_Id", userSession.Userid);
                            return RedirectToAction(nameof(Index), "Pack");

                        }
                        else
                        {
                            return Content("Fallo en la sesion");
                        }
                    }   
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
        }

        public IActionResult Register()
        { 
            return View();
        }

        public IActionResult LogOut()
        {
            if(HttpContext.Session.GetInt32("_Id")!=null)
            {
                HttpContext.Session.Remove("_Name");
                HttpContext.Session.Remove("_Id");
                return RedirectToAction(nameof(Index), "Login");
            }
            return RedirectToAction(nameof(Index), "Pack");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel model)
        {   
            if(ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = model.Name,
                    Password = CypherHelper.CifrateHash(model.Password),
                    Mail = model.Mail
                };
                Console.WriteLine(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
