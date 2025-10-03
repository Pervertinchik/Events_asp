using Events_asp.Database;
using Events_asp.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Events_asp.Controllers
{
    public class AuthentificationController : Controller
    {
        //    //Вход
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string login, string password)
        {
            User try_parse_user;
            int try_parse_userid = 0;
            using (var db = new AppDBContext())
            {
                List<User> all_users = db.users.ToList();
                try_parse_user = all_users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                try_parse_userid = db.users.Where(x => x.Login == login && x.Password == password).Select(x => x.UserId).FirstOrDefault();


            }

            if (try_parse_user != null)
            {
                HomeController.current_user_id = try_parse_userid;
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return RedirectToAction("Index", "Authentification");
            }


        }


        //Регистрация
        [HttpGet]
        public IActionResult Registration(string error)
        {
            if (error != null) ViewBag.Error = error;
            if (error == null) ViewBag.Error = " ";
            return View();
        }
        [HttpPost]
        public IActionResult Registration(string login, string password, string email)
        {
            bool isEmailDublicate = false;
            bool isLoginDublicate = false;
            using (var db = new AppDBContext())
            {

              //Проверка есть ли уже такой e-mail
                isEmailDublicate = db.users.Select(x => x.Email).Contains(email);
                if (isEmailDublicate) return RedirectToAction("Registration", "Authentification", new { error = "Пользователь с такой почтой уже зарегистрирован" });

                //Проверка есть ли уже такой login
                isLoginDublicate = db.users.Select(x => x.Login).Contains(login);
                if (isLoginDublicate) return RedirectToAction("Registration", "Authentification", new { error = "Пользователь с таким логином уже зарегистрирован" });


                db.users.Add(new User() { Login = login, Password = password, Email = email });
                db.SaveChanges();
            }

            

            return RedirectToAction("Index", "Authentification");
        }
    }
}
