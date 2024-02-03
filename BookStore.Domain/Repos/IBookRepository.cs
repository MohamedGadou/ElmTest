using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Repos
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooksAsync(int pageSize, int pageNumber, string searchKey);
    }
}
