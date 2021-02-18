using System.Text.RegularExpressions;
using Restaurant.Common;
using Restaurant.Exceptions;

namespace Restaurant.Models.Products
{
    public abstract class Product
    {
        private int size;
        private string name;
        private decimal price;
        public SizeUnit SizeUnit { get; set; }

        public string Name
        {
            get => this.name;
            set
            {
                if (!Regex.IsMatch(value, @"^[а-яА-Я ]*$"))
                {
                    throw new ValueOutOfRangeException(Messages.WrongValueMsg + $" ({value})");
                }

                this.name = value;

            }
        }

        public decimal Price
        {
            get => this.price;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ValueOutOfRangeException(Messages.WrongValueMsg + $" ({value})");
                }

                this.price = value;
            }
        }

        public int Size
        {
            get => this.size;
            set
            {
                if (value < 0 || value > 1000)
                {
                    throw new ValueOutOfRangeException(Messages.WrongValueMsg + $" ({value})");
                }

                this.size = value;
            }
        }

        
    }
}