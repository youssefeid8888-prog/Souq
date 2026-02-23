using System.ComponentModel.DataAnnotations;

namespace Souq.Models   
{
    public class ProductVm
    {
        [Required]
        public string CatName { get; set; }
        [Required]
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }


        public string ProductQty { get; set; }



    }
}
