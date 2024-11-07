using TestNinja.Mocking;

namespace TestNinjaUnitTests
{
    public class FakeFileReader : IFileReader
    {
        public string Read(string path)
        {
            return string.Empty;
        }
    }
}