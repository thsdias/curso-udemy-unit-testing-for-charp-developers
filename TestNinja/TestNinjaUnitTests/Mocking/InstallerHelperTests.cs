using System.Net;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinjaUnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IFileDownloader> _fileDownloader;
        private InstallerHelper _installerHelper;

        [SetUp]
        public void Setup()
        {
            _fileDownloader = new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_fileDownloader.Object);
        }

        [Test]
        public void DownloadInstaller_DownloadFails_ReturnsFalse()
        {
            _fileDownloader.Setup(fd =>
                    fd.DownloadFile(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<WebException>();
            
            var result = _installerHelper.DownloadInstaller("customer", "installer.exe");
            
            Assert.That(result, Is.False);            
        }

        [Test]
        public void DownloadInstaller_DownloadComplete_ReturnsTrue()
        {
            var result = _installerHelper.DownloadInstaller("customer", "installer.exe");
            
            Assert.That(result, Is.True);
        }
    }
}