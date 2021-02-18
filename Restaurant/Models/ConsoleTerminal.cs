using System;
using System.Collections.Generic;
using Restaurant.Common;
using Restaurant.Exceptions;
using Restaurant.Models.Products;

namespace Restaurant.Models
{
    internal class ConsoleTerminal : ITerminal
    {
        private readonly Dictionary<string, Type> productCategories = new Dictionary<string, Type>
        {
            {"салата", typeof(Salad)},
            {"супа", typeof(Soup)},
            {"основно ястие", typeof(MainDish)},
            {"десерт", typeof(Desert)},
            {"напитка", typeof(Drink)}
        };

        private readonly RestrauntManager restrauntManager;
        private bool isRunning;

        public ConsoleTerminal(RestrauntManager restrauntManager)
        {
            this.restrauntManager = restrauntManager;
            this.isRunning = false;
        }

        public void Start()
        {
            this.isRunning = true;
            while (this.isRunning)
            {
                this.ReadCommand();
            }
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string message, params string[] parameters)
        {
            Console.WriteLine(message, parameters);
        }

        private void ReadCommand()
        {
            var commandParams = this.ReadLine().Split(",");
            var command = commandParams[0];
            try
            {
                switch (command)
                {
                    case "салата":
                    case "супа":
                    case "основно ястие":
                    case "десерт":
                    case "напитка":
                        var product = this.CreateProduct(command, commandParams);
                        this.restrauntManager.AddProduct(product);
                        break;

                    case "продажби":
                        this.WriteLine(this.restrauntManager.PrintStatus());
                        break;

                    case {} tableNoStr when int.TryParse(command, out _) && int.Parse(command) > 0 &&
                                            int.Parse(command) <= 30:
                        var order = this.CreateOrder(tableNoStr, commandParams);
                        this.restrauntManager.Order(order);
                        break;

                    case "изход":
                        this.WriteLine(this.restrauntManager.PrintStatus());
                        this.isRunning = false;
                        break;

                    default:
                        Console.WriteLine(Messages.CommandFormatErrorMsg);
                        break;
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case CommandFormatException _:
                    case ValueOutOfRangeException _:
                        this.WriteLine(e.Message);
                        break;
                    default:
                        this.WriteLine(Messages.GeneralErrorMsg + $"\n{e.Message}");
                        break;
                }
            }
        }

        private Order CreateOrder(string tableNoStr, string[] commandParams)
        {
            var order = new Order();
            order.TableNo = int.Parse(tableNoStr);

            for (var i = 1; i < commandParams.Length; i++)
            {
                var productName = commandParams[i].Trim();
                if (this.restrauntManager.Menu.ContainsKey(productName))
                {
                    order.Products.Add(this.restrauntManager.Menu[productName]);
                }
            }

            if (order.Products.Count == 0)
            {
                throw new CommandFormatException(Messages.ProductsNotFoundMsg);
            }
            return order;
        }

        private Product CreateProduct(string command, string[] commandParams)
        {
            if(commandParams.Length!= 4)
                throw new CommandFormatException(Messages.CommandFormatErrorMsg);
            
            var productType = this.productCategories[command];
            var product = Activator.CreateInstance(productType) as Product;

            var name = commandParams[1].Trim();

            if (!int.TryParse(commandParams[2], out var size))
                throw new ValueOutOfRangeException(Messages.WrongValueMsg + $" {commandParams[2]}");

            if (!decimal.TryParse(commandParams[3], out var price))
                throw new ValueOutOfRangeException(Messages.WrongValueMsg + $" {commandParams[3]}");

            product.Name = name;
            product.Size = size;
            product.Price = price;

            return product;
        }
    }
}