using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Dtos
{
    public class BookDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public string PublishedDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
