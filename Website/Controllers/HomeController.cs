using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var obj = _context.Users.Where(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    HttpContext.Session.SetString("username", obj.Name);
                    HttpContext.Session.SetString("useremail", obj.Email);
                    return RedirectToAction("FoodItems");
                }
            }
            TempData["Message"] = "Invalid Credentials";
            return View(user);
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddFood()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditFood(int id)
        {
            return View(_context.FoodItems.Find(id));
        }

        [HttpPost]
        public IActionResult AddFood(FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                if (foodItem.Id == 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault()!;
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyTo(dataStream);
                        foodItem.ImageData = dataStream.ToArray();
                    }
                    _context.FoodItems.Add(foodItem);
                    _context.SaveChanges();
                    return RedirectToAction("FoodItems");
                }
                else
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault()!;
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyTo(dataStream);
                        foodItem.ImageData = dataStream.ToArray();
                    }
                    _context.FoodItems.Update(foodItem);
                    _context.SaveChanges();
                    return RedirectToAction("FoodItems");
                }
            }
            return View(foodItem);
        }

        [HttpPost]
        public string DeleteItem(int id)
        {
            var item = _context.FoodItems.Find(id)!;
            _context.FoodItems.Remove(item);
            _context.SaveChanges();
            return "Success";
        }


        [HttpGet]
        public IActionResult FoodItems()
        {
            if (IsLoggedIn() == true)
            {
                return View(_context.FoodItems.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult FoodItemsCards()
        {
            return View(_context.FoodItems.ToList());
        }

        [HttpGet]
        public IActionResult Cart()
        {
            if (IsLoggedIn() == true)
            {
                return View(_context.Cart.Where(a => a.AddedBy == HttpContext
                .Session.GetString("useremail")).Include(a => a.ItemFood).ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public int CartCount()
        {
            if (IsLoggedIn() == true)
            {
                return _context.Cart.Where(a => a.AddedBy == HttpContext
                .Session.GetString("useremail")).Include(a => a.ItemFood).ToList().Count;
            }
            else
            {
                return 0;
            }
        }

        [HttpGet]
        public IActionResult AddtoCart(int id)
        {
            if (IsLoggedIn() == true)
            {
                var fooditem = _context.FoodItems.Find(id)!;
                var cartitem = new CartItem
                {
                    Quantity = 1,
                    ItemID = fooditem.Id,
                    AddedBy = HttpContext.Session.GetString("useremail")!
                };
                _context.Cart.Add(cartitem);
                _context.SaveChanges();
                return RedirectToAction("Cart");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult ChangeQuantityofCartItem(int id, string operator_Sign)
        {
            var cartitem = _context.Cart.Find(id)!;
            cartitem.Quantity = operator_Sign == "+" ? cartitem.Quantity + 1 : cartitem.Quantity - 1;
            cartitem.Quantity = cartitem.Quantity < 0 ? cartitem.Quantity = 0 : cartitem.Quantity;
            if (cartitem.Quantity == 0)
            {
                _context.Cart.Remove(cartitem);
            }
            else
            {
                _context.Cart.Update(cartitem);
            }
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult RemoveCartItem(int id)
        {
            var item = _context.Cart.Find(id);
            if (item != null)
            {
                _context.Cart.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Cart");
            }
            else
            {
                return RedirectToAction("Cart");
            }
        }

        public bool IsLoggedIn()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return true;
            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}