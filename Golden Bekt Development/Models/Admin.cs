namespace Golden_Bekt_Development.Models
{
    public class Admin
    {
        public string Id { get; set; }   
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

    }
}
