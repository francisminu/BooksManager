using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.RequestModels;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);
        Task<IEnumerable<Book>> GetBooksByCountAsync(GetBooksByPageSizeRequestModel requestModel);
        Task<Book> UpdateBooksAsync(UpdateBookRequestModel requestModel);
        Task<IEnumerable<Book>> AddBookAsync(Book newBook);
        Task<int> GetTotalNumberOfBooks();
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
            
        }

        public async Task<int> GetTotalNumberOfBooks()
        {
            return await _dataContext.Books.CountAsync();
        }


        public async Task<IEnumerable<Book>> GetBooksByCountAsync(GetBooksByPageSizeRequestModel requestModel)
        {
            var queryable = _dataContext.Books.AsQueryable();
            var filterByTitle = requestModel.FilterOn.Any(x => x.Contains("Title"));
            var filterByDescription = requestModel.FilterOn.Any(x => x.Contains("Description"));
            var filteredData = queryable.Where(x => (filterByTitle ? x.Title.Contains(requestModel.FilterBy) : true)
            && (filterByDescription ? x.ShortDescr.Contains(requestModel.FilterBy) : true));
            return await filteredData.OrderBy(on => on.Title)
            .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
            .Take(requestModel.PageSize)
            .ToListAsync();
            
        }

        public async Task<Book> UpdateBooksAsync(UpdateBookRequestModel requestModel)
        {
            var bookItem = await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == requestModel.BookId);
            if(bookItem != null)
            {
                bookItem.ShortDescr = requestModel.ShortDescription;
                bookItem.ThumbnailUrl = requestModel.ThumbnailUrl;
                await _dataContext.SaveChangesAsync();
            }
            return bookItem;
        }

        public async Task<IEnumerable<Book>> AddBookAsync(Book newBook)
        {
            var record = await _dataContext.Books.AddAsync(newBook);
            await _dataContext.SaveChangesAsync();
            var queryable = _dataContext.Books.AsQueryable();
            var pageSize = 10;
            
            return await queryable.OrderBy(on => on.Title)
            .Take(pageSize)
            .ToListAsync();
        }
    }
}
