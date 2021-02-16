
using System;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace GradeBook.Tests
{
    
    public class IntegrationTests
    {
        private StringBuilder sb;
        private StringWriter sw;
        public IntegrationTests()
        {
            this.sb = new StringBuilder();
            this.sw = new StringWriter(sb);
            Console.SetOut(sw);
        }
        [Theory]
        [InlineData("test1-input.txt", "test1-expected.txt")]
        public void MainTest(string testInputFile, string testExpectedFile)
        {
            string input = File.ReadAllText(testInputFile);
            var reader = new StringReader(input);

            var expected = File.ReadAllText(testExpectedFile);
            
            Console.SetIn(reader);
            
            Thread t = new Thread(Program.Main);
            t.Start();
            
            Thread.Sleep(100);
            Assert.Equal(expected, sb.ToString());
            t.Join();
        }
    }
}
