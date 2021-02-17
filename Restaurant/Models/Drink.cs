namespace Restaurant.Models
{
    public class Drink : Product
    {
        private const double CalorieMultiplier = 1.5;

        public Drink(double volume)
        {
            this.SizeUnit = SizeUnit.Milliliters;
        }


        public double Calories => this.Size * CalorieMultiplier;
    }
}