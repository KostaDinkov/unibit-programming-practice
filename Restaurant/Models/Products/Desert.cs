namespace Restaurant.Models.Products
{
    internal class Desert : Product
    {
        private const double CalorieMultiplier = 3;
      

        public Desert()
        {
            this.SizeUnit = SizeUnit.Grams;
           
        }
        
        public double Calories => this.Size * CalorieMultiplier;
    }
}