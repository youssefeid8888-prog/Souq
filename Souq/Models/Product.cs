using System;
using System.Collections.Generic;

namespace Souq.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Catid { get; set; }

    public string? Photo { get; set; }

    public string? Type { get; set; }

    public string? SupplierName { get; set; }

    public DateOnly? EntryData { get; set; }

    public string? ReviewUrl { get; set; }

    public int? Quantity { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Catigory? Cat { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
