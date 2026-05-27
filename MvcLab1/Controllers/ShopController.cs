using Microsoft.AspNetCore.Mvc;

namespace MvcLab1.Controllers
{
    [Route("store")]
    [Route("shop")]
    public class ShopController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.StoreName = "Супер-магазин";
            ViewData["ProductsCount"] = 150;
            return View();
        }

        [Route("category/{categoryName}")]
        public IActionResult Category(string categoryName)
        {
            ViewBag.Category = categoryName;

            var products = new List<string>();
            switch (categoryName.ToLower())
            {
                case "electronics":
                    products.Add("Смартфон");
                    products.Add("Ноутбук");
                    products.Add("Планшет");
                    break;
                case "books":
                    products.Add("Война и мир");
                    products.Add("Преступление и наказание");
                    products.Add("Мастер и Маргарита");
                    break;
                default:
                    products.Add("Товар 1");
                    products.Add("Товар 2");
                    products.Add("Товар 3");
                    break;
            }

            ViewBag.Products = products;
            return View();
        }

        [Route("product/{id:int}/details")]
        public IActionResult ProductDetails(int id)
        {
            ViewBag.ProductId = id;
            ViewBag.ProductName = $"Товар #{id}";
            return View();
        }
    }
}