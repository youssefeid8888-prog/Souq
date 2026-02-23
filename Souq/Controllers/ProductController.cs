using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Souq.Models;

namespace Souq.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            SouqcomContext db = new SouqcomContext();
            var list = db.Catigories.Select(x => new { x.Id, x.Name }).ToList();

            ViewBag.CatList = new SelectList(list, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductVm model)
        {
            if (ModelState.IsValid)
            {
                SouqcomContext db = new SouqcomContext();
                Catigory c = new Catigory();

                c.Name = model.CatName;



                db.Products.Add(new Product
                {
                    Name = model.ProductName,
                    Price = model.ProductPrice,
                    Quantity = int.Parse(model.ProductQty),

                    Cat = c

                });
                db.SaveChanges();

                return View("Index");
            }
            return View("Index",model);
        }

    }
}
