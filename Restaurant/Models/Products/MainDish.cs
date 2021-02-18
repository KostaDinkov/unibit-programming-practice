namespace Restaurant.Models.Products
{
    internal class MainDish : Product
    {
        private const double CalorieMultiplier = 1;

        public MainDish()
        {
            this.SizeUnit = SizeUnit.Grams;
        }

        public double Calories => this.Size * CalorieMultiplier;
    }
}