public class ProgramCreateDto
{

    public string ProgramName { get; set; } = string.Empty;
    public IFormFile? ProgramUrl { get; set; }
    public string ProgramDescription { get; set; } = string.Empty;
}
