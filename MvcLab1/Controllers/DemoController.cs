using Microsoft.AspNetCore.Mvc;

namespace MvcLab1.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Hello()
        {
            return Content("Привет из DemoController!");
        }

        public IActionResult Greeting(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Content("Привет, незнакомец!");
            else
                return Content($"Привет, {name}!");
        }

        public IActionResult ShowView()
        {
            // Использование ViewBag
            ViewBag.UserName = "Елена";
            ViewBag.RegistrationDate = new DateTime(2026, 2, 04);

            // Использование ViewData
            ViewData["PageTitle"] = "Информационная страница";
            ViewData["VisitCount"] = 42;

            return View();
        }

        public IActionResult UserInfo(string name, int age)
        {
            ViewBag.UserName = name ?? "Неизвестный";
            ViewBag.Age = age;
            ViewBag.IsAdult = age >= 18;
            ViewData["CurrentYear"] = DateTime.Now.Year;
            ViewData["BirthYear"] = DateTime.Now.Year - age;
            return View();
        }
    }
}