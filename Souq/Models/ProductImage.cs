using System;
using System.Collections.Generic;

namespace Souq.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public string? Image { get; set; }

    public virtual Product? Product { get; set; }
}
