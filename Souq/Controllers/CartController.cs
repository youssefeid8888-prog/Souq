using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Souq.Models;


namespace Souq.Controllers
{
    public class CartController : Controller
    {
        private readonly SouqcomContext db;

        public CartController(SouqcomContext context)
        {
            db = context;
        }
        
        public IActionResult AddToCart(int productId)
        {
            string userId;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.Name;
            }
            else
            {
                if (HttpContext.Session.GetString("UserId") == null)
                {
                    HttpContext.Session.SetString("UserId", Guid.NewGuid().ToString());
                }

                userId = HttpContext.Session.GetString("UserId");
            }

            // 🔥 شوف المنتج موجود قبل كده ولا لأ
            var cartItem = db.Carts
                .FirstOrDefault(c => c.Productid == productId && c.Userid == userId);

            if (cartItem != null)
            {
                // المنتج موجود → زود الكمية
                cartItem.Qty += 1;
            }
            else
            {
                // المنتج مش موجود → ضيفه جديد
                db.Carts.Add(new Cart
                {
                    Productid = productId,
                    Userid = userId,
                    Qty = 1
                });
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }




        public IActionResult Index()
        {

            string userId;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.Name;
            }
            else
            {
                userId = HttpContext.Session.GetString("UserId");
            }

            
            var cartItems = db.Carts
                .Include(c => c.Product)
                .Where(c => c.Userid == userId)
                .ToList();

            return View(cartItems);
        }

        public IActionResult Remove(int id)
        {
            var item = db.Carts.Find(id);
            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // زيادة الكمية
        public IActionResult IncreaseQuantity(int id)
        {
            var cartItem = db.Carts.Find(id);
            if (cartItem != null)
            {
                cartItem.Qty++;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // نقص الكمية
        public IActionResult DecreaseQuantity(int id)
        {
            var cartItem = db.Carts.Find(id);
            if (cartItem != null && cartItem.Qty > 1)
            {
                cartItem.Qty--;
                db.SaveChanges();
            }
            else if (cartItem != null && cartItem.Qty == 1)
            {
                // اختياري: إذا نقصت عن 1 يتم حذف المنتج
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(string payment)
        {
            return Content("تم اختيار: " + payment);
        }



    }
}
