using System.ComponentModel.DataAnnotations;

namespace Golden_Bekt_Development.Models
{
    public class Common
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int? IsActive { get; set; } = 1;
        public int? IsDelete { get; set; } = 0;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
