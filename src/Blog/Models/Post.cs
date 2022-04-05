namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public Category Category { get; set; }
        public User Author { get; set; }

        // virtual: allow an item to be overriden and enable entity framework lazy loading for this item
        public virtual IList<Tag> Tags { get; set; }
    }
}