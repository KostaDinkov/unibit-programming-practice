namespace Restaurant.Common
{
    public class Messages
    {
        //error messages
        public const string CommandFormatErrorMsg = "Грешен формат на командата.";
        public const string GeneralErrorMsg = "Възникна грешка по време на изпълнението.";
        public const string WrongValueMsg = "Грешна стойност.";
        public const string ProductsNotFoundMsg = "Нито един от продуктите в поръчката не беше намерен в менюто";

        public const string NoSales = "Няма продажби за деня";
        //sales stats
        public const string TotalTablesMsg = "Общо заети маси през деня: {0}";
        public const string TotalSales = "Общо продажби: {0} – {1:F2}";
        public const string ByCategory = "По категории:";
        
    }
}