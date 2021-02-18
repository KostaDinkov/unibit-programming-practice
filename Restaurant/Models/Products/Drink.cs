namespace Restaurant.Models.Products
{
    public class Drink : Product
    {
        private const double CalorieMultiplier = 1.5;

        public Drink()
        {
            this.SizeUnit = SizeUnit.Milliliters;
        }


        public double Calories => this.Size * CalorieMultiplier;
    }
}