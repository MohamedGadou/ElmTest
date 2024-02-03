using BookStore.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.IService
{
    public interface IBookAppService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageSize, int pageNumber, string searchKey);
    }
}
