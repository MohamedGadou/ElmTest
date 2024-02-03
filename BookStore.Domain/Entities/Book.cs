namespace BookStore.Domain.Entities
{
    public class Book
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
