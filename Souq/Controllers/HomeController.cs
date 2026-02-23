using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Souq.Models;

namespace Souq.Controllers
{
    public class HomeController : Controller
    {
        SouqcomContext db = new SouqcomContext();

        public IActionResult Index()
        {
            indexVM result = new indexVM();

            result.SliderProducts = db.Products.Take(6).ToList();
            result.Catigoryies = db.Catigories.ToList();
            result.Products = db.Products.Skip(6).ToList();
            result.Reviews = db.Reviews.ToList();
            result.LatestProducts = db.Products.OrderByDescending(x=>x.EntryData).Take(4).ToList();
            result.SpasficCat = db.Products.Where(x => x.Catid == 2).Take(6).ToList();



            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Cart()
        {
            return View();
        }

            
        public IActionResult Categories ()
        {
            var cats = db.Catigories.ToList();
            ViewBag.isAdmin = true;

            return View(cats);
        }


        public IActionResult Products(int id)
        {
            var products = db.Products.Where(x=> x.Catid == id).ToList();        
            return View(products);
        }

        public IActionResult currentProduct(int id)
        {
            //var product = db.Products.Include(x=>x.Cat).Include(x=>x.ProductImages).FirstOrDefault(x => x.Id == id);

            var product = db.Products
        .Include(p => p.Cat)
        .Include(p => p.ProductImages)
        .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            // جلب المنتجات المشابهة (نفس القسم وبحد أقصى 10 منتجات مثلاً)
            ViewBag.RelatedProducts = db.Products
                .Where(p => p.Cat.Id == product.Cat.Id && p.Id != id)
                .Take(10)
                .ToList();




            return View(product);
        }


        [HttpGet]
        public IActionResult ProductSearch(string xname)
        {
            var Products = new List<Product>();

            if (string.IsNullOrEmpty(xname))
                Products = db.Products.ToList();
            else
                Products = db.Products.Where(x => x.Name.Contains(xname)).ToList(); 


            return View(Products);
        }



        [HttpPost]
        public IActionResult SendReview(ReviewVM mpdel)
        {
            db.Reviews.Add(new Review { Name = mpdel.Name, Email = mpdel.Email, Subject = mpdel.Subject, Description = mpdel.Description });
            db.SaveChanges();

            return RedirectToAction("Index");
        }

























        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
