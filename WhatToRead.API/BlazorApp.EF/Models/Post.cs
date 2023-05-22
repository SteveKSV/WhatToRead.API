namespace BlazorApp.EF.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Views { get; set; } = 0;
        public string Image { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool Published { get; set; } = false;
        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;
    }
}
