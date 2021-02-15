
namespace GradeBook
{
    interface ITerminal
    {
        public bool IsRunning { get; set; }
        public void ReadCommand();

        public void Log(string message);

        

        

    }
}
