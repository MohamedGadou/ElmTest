using BookStore.Application.Dtos;
using BookStore.Application.IService;
using BookStore.Domain.Repos;

namespace BookStore.Application.Service
{
    public class BookAppService : IBookAppService
    {
        private readonly IBookRepository _booksRepo;

        public BookAppService(IBookRepository bookRepository)
        {
            _booksRepo = bookRepository;
        }
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageSize, int pageNumber, string searchKey)
        {
            var books = await _booksRepo.GetBooksAsync(pageSize, pageNumber, searchKey);
            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Cover = b.Cover,
                Description = b.Description,
                LastModified = b.LastModified,
                PublishedDate = b.PublishedDate,
            });
        }
    }
}
