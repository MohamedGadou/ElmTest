using BookStore.Application.Dtos;
using BookStore.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookAppService _booksService;

        public BooksController(IBookAppService bookAppService)
        {
            _booksService = bookAppService;
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageSize, int pageNumber, string searchKey)
        {
            return await _booksService.GetAllBooksAsync(pageSize, pageNumber, searchKey);
        }
    }
}
