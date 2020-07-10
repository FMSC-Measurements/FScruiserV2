using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace FScruiser.Core.Test
{
    public class TestBase
    {
        private string _testTempPath;
        protected ITestOutputHelper Output { get; private set; }
        protected Stopwatch _stopwatch;

        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            Output.WriteLine($"CodeBase: {System.Reflection.Assembly.GetExecutingAssembly().CodeBase}");

            var testTempPath = TestTempPath;
            if (!Directory.Exists(testTempPath))
            {
                Directory.CreateDirectory(testTempPath);
            }
        }

        public string TestExecutionDirectory
        {
            get
            {
                var codeBase = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                return Path.GetDirectoryName(codeBase);
            }
        }

        public string TestFilesDirectory => Path.Combine(TestExecutionDirectory, "TestFiles");



        public string TestTempPath
        {
            get
            {
                return _testTempPath ?? (_testTempPath = Path.Combine(Path.GetTempPath(), "TestTemp", this.GetType().FullName));
            }
        }

        public string GetTestFile(string fileName)
        {
            var sourceTestFilePath = Path.Combine(TestFilesDirectory, fileName);
            if (File.Exists(sourceTestFilePath) == false) { throw new FileNotFoundException(sourceTestFilePath); }

            var testTemp = TestTempPath;
            var destFilePath = Path.Combine(testTemp, fileName);

            File.Copy(sourceTestFilePath, destFilePath, true);
            return destFilePath;
        }

        public void StartTimer()
        {
            _stopwatch = new Stopwatch();
            Output.WriteLine("Stopwatch Started");
            _stopwatch.Start();
        }

        public void EndTimer()
        {
            _stopwatch.Stop();
            Output.WriteLine("Stopwatch Ended:" + _stopwatch.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
