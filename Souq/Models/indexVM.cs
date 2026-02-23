namespace Souq.Models
{
    public class indexVM
    {

        public indexVM()
        {
            Catigoryies = new List<Catigory>();
            Products = new List<Product>();
            Reviews = new List<Review>();
            LatestProducts = new List<Product>();
            SliderProducts = new List<Product>();

        }


        public List<Catigory> Catigoryies { get; set; }
        public List<Product> Products { get; set; }

        public List<Product> LatestProducts { get; set; }

        public List<Product> SliderProducts { get; set; }

        public List<Product> SpasficCat {  get; set; }


        public List<Review> Reviews { get; set; }
    }
}
