namespace Golden_Bekt_Development.DTOs
{
    public class Our_newsDto
    {
        public string? NewsTitle { get; set; }
        public IFormFile? NewsUrl { get; set; }
        public string? NewsDescription { get; set; }
        public DateTime? NewsDate { get; set; }
    }
}
