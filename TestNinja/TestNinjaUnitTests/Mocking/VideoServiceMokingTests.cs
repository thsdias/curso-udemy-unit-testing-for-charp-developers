using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinjaUnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceMokingTests
    {
        private Mock<IFileReader> _fileReader;
        private VideoService _videoService;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _videoService = new VideoService(_fileReader.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(f => f.Read("video.txt")).Returns(string.Empty);

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
    }
}