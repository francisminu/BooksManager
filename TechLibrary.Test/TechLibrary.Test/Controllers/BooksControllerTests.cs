using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLibrary.RequestModels;
using TechLibrary.Services;

namespace TechLibrary.Controllers.Tests
{
    [TestFixture()]
    [Category("ControllerTests")]
    public class BooksControllerTests
    {

        private  Mock<ILogger<BooksController>> _mockLogger;
        private  Mock<IBookService> _mockBookService;
        private  Mock<IMapper> _mockMapper;
        private NullReferenceException _expectedException;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _expectedException = new NullReferenceException("Test Failed...");
            _mockLogger = new Mock<ILogger<BooksController>>();
            _mockBookService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();
        }

        //[Test()]
        //public async Task GetAllTest()
        //{
        //    //  Arrange
        //    _mockBookService.Setup(b => b.GetBooksAsync()).Returns(Task.FromResult(It.IsAny<List<Domain.Book>>()));
        //    var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

        //    //  Act
        //    var result = await sut.GetAll();

        //    //  Assert
        //    _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");
        //}

        [Test()]
        public async Task GetByIdTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBookByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Domain.Book>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var requestModel = new GetBookRequestModel
            {
                BookId = 10
            };
          
            var result = await sut.GetById(requestModel);
            Assert.NotNull(result);
        }

        [Test()]
        public async Task GetBooksByPageSizeTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBooksByCountAsync(It.IsAny<GetBooksByPageSizeRequestModel>())).Returns(Task.FromResult(It.IsAny<IEnumerable<Domain.Book>>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var requestModel = new GetBooksByPageSizeRequestModel
            {
                PageSize = 10,
                PageNumber = 1
            };

            var result = await sut.GetBooksByPageSize(requestModel);
            
            _mockBookService.Verify(s => s.GetBooksByCountAsync(It.IsAny<GetBooksByPageSizeRequestModel>()), Times.Once, "Expected GetBooksAsync to have been called once");
        }

        [Test()]
        public async Task UpdateBookDetailsTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.UpdateBooksAsync(It.IsAny<UpdateBookRequestModel>())).Returns(Task.FromResult(It.IsAny<Domain.Book>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var requestModel = new UpdateBookRequestModel
            {
                BookId = 10,
                ShortDescription = "Test"
            };

            var result = await sut.UpdateBookDetails(requestModel);

            _mockBookService.Verify(s => s.UpdateBooksAsync(It.IsAny<UpdateBookRequestModel>()), Times.Once, "Expected GetBooksAsync to have been called once");
        }
    }
}