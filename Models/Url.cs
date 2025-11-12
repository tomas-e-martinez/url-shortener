namespace urlShortenerApi.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string Original { get; set; }
        public string Shortened { get; set; }
    }
}
