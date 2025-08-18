namespace Golden_Bekt_Development.DTOs
{
    public class Commercial_registerDto
    {
        public string? CommercialRegisterNumber { get; set; }
        public IFormFile? CommercialRegisterURL { get; set; }
        public DateTime? CommercialRegisterDate { get; set; }
        public string? CommercialRegisterDescription { get; set; }
    }
}
