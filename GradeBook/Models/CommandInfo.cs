namespace GradeBook.Models
{
    public class CommandInfo
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }

        public string Example { get; set; }

        public override string ToString()
        {
            return
                $"{this.Name}\nFormat: {this.Format}\nDescription: {this.Description}\nExample: {this.Example}";
        }
    }
}