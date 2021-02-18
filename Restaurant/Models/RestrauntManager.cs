using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Restaurant.Common;
using Restaurant.Models.Products;


namespace Restaurant.Models
{
    public class RestrauntManager
    {
        private IDictionary<string, Product> menu;
        private IDictionary<int, List<Order>> tables;
        private decimal totalMoney = 0;
        private decimal totalSales = 0;

        public RestrauntManager()
        {
            this.menu = new Dictionary<string, Product>();
            this.tables = new Dictionary<int, List<Order>>();
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

        public void Order( Order order)
        {
            if (this.tables.ContainsKey(order.TableNo))
            {
                this.tables[order.TableNo].Add(order);
            }
            else
            {
                this.tables.Add(order.TableNo, new List<Order>{order});
            }

            this.totalMoney += order.Products.Select(o => o.Price).Sum();
            this.totalSales += order.Products.Count;

        }

        public string PrintStatus()
        {
            if (this.tables.Count == 0) return Messages.NoSales;
            
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(Messages.TotalTablesMsg, this.tables.Count));
            sb.AppendLine(string.Format(Messages.TotalSales, this.totalSales, this.totalMoney));
            sb.AppendLine(Messages.ByCategory);

            var tup = (1, 4m);
            tup.Item1 += 2;
            tup.Item2 += 5;

            var grouped = new Dictionary<string, (int count, decimal price)>();
            foreach (var table in this.tables)
            {
                foreach (var order in table.Value)
                {
                    foreach (var product in order.Products)
                    {
                        //NOTE: another way of doing this was to add a category property to the products and initialize it upon constructing
                        //with the correct string, but I just wanted to test the new C# pattern matching capabilities
                        var category = product switch
                        {
                            Salad _ => "Салата",
                            Soup _ => "Супа",
                            MainDish _ => "Основно ястие",
                            Desert _ => "Десерт",
                            Drink _ => "Напитка",
                            _ => "No Category"
                        };

                        if (grouped.ContainsKey(category))
                        {
                            var newCount = grouped[category].count + 1;
                            var newPrice = grouped[category].price + product.Price;
                            grouped[category]=  (newCount,newPrice) ;
                        }
                        else
                        {
                            grouped.Add(category,(1,product.Price));

                        }
                    }
                }
            }

            foreach (var entry in grouped)
            {
                sb.AppendLine($"  -  {entry.Key}: {entry.Value.count} - {entry.Value.price:F2}");
            }
            return sb.ToString().Trim('\n','\r');

        }

        public IDictionary<string, Product> Menu => this.menu;



    }
}
