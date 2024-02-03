using BookStore.Domain.Entities;
using BookStore.Domain.Repos;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BookStore.Infrastructure.Repos
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<Book>> GetBooksAsync(int pageSize, int pageNumber, string searchKey)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("rowOffset", (pageSize * (pageNumber - 1)));
            parameters.Add("fetchNextRows", pageSize);
            parameters.Add("searchKey", searchKey);

            var books = (await connection.QueryAsync<Book>(
                            "Books_GetAll",
                            parameters,
                            commandType: CommandType.StoredProcedure)).ToList();

            return books;
        }
    }
}
