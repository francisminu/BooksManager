using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using TechLibrary.RequestModels;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            //var books = await _bookService.GetBooksAsync();
            var requestModel = new GetBooksByPageSizeRequestModel
            {
                PageNumber = 1,
                PageSize = 10
            };
            var books = await _bookService.GetBooksByCountAsync(requestModel);

            var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        

        [HttpPut]
        [Route("getbyid")]
        public async Task<IActionResult> GetById(GetBookRequestModel requestModel)
        {
            _logger.LogInformation($"Get book by id {requestModel.BookId}");

            var book = await _bookService.GetBookByIdAsync(requestModel.BookId);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPut]
        [Route("getBooksByPageSize")]
        public async Task<IActionResult> GetBooksByPageSize(GetBooksByPageSizeRequestModel requestModel)
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksByCountAsync(requestModel);

            var bookResponse = _mapper.Map<List<BookResponse>>(books);
            var totalNumberOfBooks = await _bookService.GetTotalNumberOfBooks();
            var getBooksResponse = new GetBooksResponseModel
            {
                BookResponses = bookResponse,
                TotalNumberOfBooks = totalNumberOfBooks
            };
            return Ok(getBooksResponse);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateBookDetails(UpdateBookRequestModel requestModel)
        {
            _logger.LogInformation("Update book details");

            var updatedBook = await _bookService.UpdateBooksAsync(requestModel);
            var updatedBookResponse = _mapper.Map<BookResponse>(updatedBook);

            return Ok(updatedBookResponse);
        }

        [HttpPut]
        [Route("add")]
        public async Task<IActionResult> AddBook(AddBookRequestModel requestModel)
        {
            _logger.LogInformation("Update book details");
            var newBook = _mapper.Map<Book>(requestModel);
            var updatedBook = await _bookService.AddBookAsync(newBook);
            return Ok(updatedBook);
        }
    }
}
