namespace Restaurant.Models
{
    internal interface ITerminal
    {
        public void WriteLine(string message);
        public string ReadLine();

        public void WriteLine(string message, params string[] parameters);

        public void Start();
        
    }
}