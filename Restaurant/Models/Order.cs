using System.Collections.Generic;
using Restaurant.Models.Products;

namespace Restaurant.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new List<Product>();
        }

        public int TableNo { get; set; }
        public IList<Product> Products { get; set; }
    }
}