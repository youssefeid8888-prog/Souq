using Souq.Models;
using System.Collections.Generic;

public class OrderVM
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public List<Cart> Items { get; set; }

    public decimal TotalAmount { get; set; }
}