using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Souq.Models;
using System;
using System.Linq;

public class CheckoutController : Controller
{
    private readonly SouqcomContext _context;

    public CheckoutController(SouqcomContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult CashOnDelivery()
    {
        var cartItems = _context.Carts
    .Include(c => c.Product)
    .ToList();
        var vm = new OrderVM
        {
            Items = cartItems,
            TotalAmount = cartItems?.Sum(x => (x.Product?.Price ?? 0) * x.Qty) ?? 0
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult CashOnDelivery(OrderVM model)
    {
        // 1. جلب محتويات السلة الحالية
        var cartItems = _context.Carts.Include(c => c.Product).ToList();

        // 2. حفظ بيانات الطلب الأساسية
        var order = new Order
        {
            FullName = model.FullName,
            Email = model.Email,
            Addres = model.Address,
            OrderDate = DateTime.Now,
            PaymentMethod = "Cash On Delivery",
            TotalAmount = cartItems.Sum(x => (x.Product?.Price ?? 0) * x.Qty)
        };
        _context.Orders.Add(order);
        _context.SaveChanges(); // هنا الـ Order بياخد ID من الداتابيز

        // 3. حفظ كل منتج من السلة في جدول OrderItems
        foreach (var item in cartItems)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,       // ربط التفاصيل بالطلب
                ProductId = item.Product.Id,
                Quantity = item.Qty ?? 0,
                UnitPrice = item.Product?.Price ?? 0
            };
            _context.OrderItems.Add(orderItem);
        }

        // 4. مسح السلة بعد ما نقلنا البيانات للجدول الدائم
        _context.Carts.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("Invoice", new { id = order.Id });
    }












    //// POST
    //[HttpPost]
    //public IActionResult CashOnDelivery(OrderVM model)
    //{
    //    // 1. لازم نستخدم Include عشان بيانات المنتج (السعر) تظهر
    //    var cartItems = _context.Carts
    //        .Include(c => c.Product)
    //        .ToList();

    //    // تأكد إن السلة مش فاضية قبل ما تعمل Order
    //    if (!cartItems.Any())
    //    {
    //        return RedirectToAction("Index", "Cart"); // أو وديه لصفحة السلة
    //    }

    //    var order = new Order
    //    {
    //        FullName = model.FullName,
    //        Email = model.Email,
    //        Addres = model.Address,
    //        PaymentMethod = "Cash On Delivery",
    //        OrderDate = DateTime.Now,
    //        // 2. يفضل استخدام التحقق من الـ null هنا أيضاً للأمان
    //        TotalAmount = cartItems.Sum(x => (x.Product?.Price ?? 0) * x.Qty)
    //    };

    //    _context.Orders.Add(order);

    //    // 3. (اختياري) يفضل مسح السلة بعد إتمام الطلب بنجاح
    //    // _context.Carts.RemoveRange(cartItems);

    //    _context.SaveChanges();

    //    return RedirectToAction("Invoice", new { id = order.Id });
    //}







    public IActionResult Invoice(int id)
    {
        // 1. جلب بيانات الطلب مع التفاصيل والمنتجات
        var order = _context.Orders
            .Include(o => o.OrderItems)        // تحميل تفاصيل الطلب
            .ThenInclude(oi => oi.Product)    // تحميل بيانات المنتج (الاسم، السعر، إلخ)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // 2. ملء الـ ViewModel بالبيانات كاملة
        var vm = new OrderVM
        {
            FullName = order.FullName,

            // السطرين دول هما اللي ناقصين عندك:
            Email = order.Email,           // نقل الإيميل
            Address = order.Addres,        // نقل العنوان (تأكد من حرف الـ s الناقص في قاعدة البيانات)

            TotalAmount = order.TotalAmount ?? 0,

            // تحويل OrderItems لـ Cart عشان الـ View تفهمها
            Items = order.OrderItems.Select(oi => new Cart
            {
                Product = oi.Product,
                Qty = oi.Quantity
            }).ToList()
        };

        return View(vm);
    }




    //public IActionResult Invoice(int id)
    //{
    //    // جلب بيانات الطلب من الداتابيز
    //    //var order = _context.Orders.FirstOrDefault(o => o.Id == id);
    //    //if (order == null) return NotFound();

    //    //// جلب محتويات السلة (عشان نعرضها في الفاتورة قبل ما نمسحها أو كمرجع)
    //    //var cartItems = _context.Carts.Include(c => c.Product).ToList();

    //    var order = _context.Orders
    //    .Include(o => o.OrderItems)        // هات التفاصيل
    //    .ThenInclude(oi => oi.Product)    // هات اسم المنتج وصورته
    //    .FirstOrDefault(o => o.Id == id);


    //    var vm = new OrderVM
    //    {
    //        FullName = order.FullName,
    //        TotalAmount = order.TotalAmount ?? 0,
    //        // تحويل OrderItems لـ Cart عشان الـ View تفهمها
    //        Items = order.OrderItems.Select(oi => new Cart
    //        {
    //            Product = oi.Product,
    //            Qty = oi.Quantity
    //        }).ToList()
    //    };


    //    //var vm = new OrderVM
    //    //{
    //    //    FullName = order.FullName,
    //    //    Email = order.Email,
    //    //    Address = order.Addres, // تأكد لو كانت Address في الـ VM و Addres في الداتابيز
    //    //    TotalAmount = order.TotalAmount??0,
    //    //    Items = cartItems // نمرر المنتجات هنا عشان الـ foreach تشتغل

    //    //};
    //    //_context.Carts.RemoveRange(order);
    //    //_context.SaveChanges();

    //    return View(vm);
    //}
    public IActionResult MyOrders()
    {
        // بنجيب البيانات وبنعمل Include للمنتجات لو حبيت تعرض تفاصيل أكتر مستقبلاً
        var orders = _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }
}