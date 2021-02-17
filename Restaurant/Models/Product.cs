using System.Text.RegularExpressions;

namespace Restaurant.Models
{
    public class Product
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
                if (!Regex.IsMatch(value, @"[a-zA-Z]*"))
                {
                    throw new ValueOutOfRangeException();
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
                    throw new ValueOutOfRangeException();
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
                    throw new ValueOutOfRangeException();
                }

                this.size = value;
            }
        }
    }
}