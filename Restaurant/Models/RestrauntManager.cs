using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Restaurant.Models
{
    public class RestrauntManager
    {
        private IDictionary<string, Product> menu;
        private IDictionary<int, List<Product>> tables;

        public RestrauntManager()
        {
            this.menu = new Dictionary<string, Product>();
            this.tables = new Dictionary<int, List<Product>>();
        }

        public void AddProduct(Product product)
        {
            if (!this.menu.ContainsKey(product.Name))
            {
                this.menu[product.Name] = product;
            }
            else
            {
                this.menu.Add(product.Name, product);
            }
        }

        public void Order(int table, List<Product>products)
        {
            if (this.tables.ContainsKey(table))
            {
                this.tables[table] = products;
            }
            else
            {
                this.tables.Add(table, products);
            }
        }

        public string PrintStatus()
        {
            throw new NotFiniteNumberException();
        }


    }
}
