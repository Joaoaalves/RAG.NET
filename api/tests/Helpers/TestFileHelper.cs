using System.Runtime.CompilerServices;

namespace tests.Helpers
{
    public static class TestFileHelper
    {
        public static string GetTestFilePath(string filename, [CallerFilePath] string callerFilePath = "")
        {
            var testDir = Path.GetDirectoryName(callerFilePath)!;
            var testFilesPath = Path.Combine(testDir, "..", "TestFiles", filename);
            return Path.GetFullPath(testFilesPath);
        }
    }
}
