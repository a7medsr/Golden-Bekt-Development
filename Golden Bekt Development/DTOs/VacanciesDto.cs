namespace Golden_Bekt_Development.DTOs
{
    public class VacanciesDto
    {
        public string? VacancyName { get; set; }
        public IFormFile? VacancyURL { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? ApplicationDeadline { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
    }
}
