using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinjaUnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        /*
        /// <summary>
        /// Dependency Injection via Method Parameters.
        /// </summary>
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            var service = new VideoService();

            var result = service.ReadVideoTitle(new FakeFileReader());

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
        */

        /*
        /// <summary>
        /// Dependency Injection via Properties.
        /// </summary>
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            var service = new VideoService();
            service.FileReader = new FakeFileReader();

            var result = service.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
        */

        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _repository;
        private VideoService _videoService;

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
            _repository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _repository.Object);
        }
        
        /// <summary>
        /// Dependency Injection via Constructor.
        /// </summary>
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            var service = new VideoService(new FakeFileReader());

            var result = service.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnAnEmptyString()
        {
            _repository.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>());
            
            var result = _videoService.GetUnprocessedVideosAsCsv();
            
            Assert.That(result, Is.EqualTo(""));
        }
        
        [Test]
        public void GetUnprocessedVideosAsCsv_AFewUnprocessedVideos_ReturnStringWithIdOfUnprocessedVideos()
        {
            _repository.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>
            { 
                new Video {Id = 1},
                new Video {Id = 2},
                new Video {Id = 3}
            });
            
            var result = _videoService.GetUnprocessedVideosAsCsv();
            
            Assert.That(result, Is.EqualTo("1,2,3"));
        }
    }
}