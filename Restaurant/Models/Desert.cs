namespace Restaurant.Models
{
    internal class Desert : Product
    {
        private const double CalorieMultiplier = 3;
      

        public Desert(double weight)
        {
            this.SizeUnit = SizeUnit.Grams;
           
        }
        
        public double Calories => this.Size * CalorieMultiplier;
    }
}