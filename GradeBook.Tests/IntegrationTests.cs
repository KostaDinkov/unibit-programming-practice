﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace GradeBook.Tests
{
    public class IntegrationTests
    {
        private readonly StringBuilder sb;
        private readonly StringWriter sw;

        public IntegrationTests()
        {
            this.sb = new StringBuilder();
            this.sw = new StringWriter(this.sb);
            Console.SetOut(this.sw);
        }

        [Theory]
        [InlineData("test1-input.txt", "test1-expected.txt")]
        [InlineData("test2-input.txt", "test2-expected.txt")]

        //todo: add some more test cases
        public void MainTest(string testInputFile, string testExpectedFile)
        {
            var input = File.ReadAllText(testInputFile);
            var reader = new StringReader(input);

            var expected = File.ReadAllText(testExpectedFile);

            Console.SetIn(reader);

            var t = new Thread(Program.Main);
            t.Start();

            Thread.Sleep(100);
            Assert.Equal(expected, this.sb.ToString());
            t.Join();
        }
    }
}