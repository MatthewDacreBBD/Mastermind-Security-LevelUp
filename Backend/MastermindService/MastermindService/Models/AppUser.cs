namespace MastermindService.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string? passwordSalt { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public string? authenticationToken { get; set; }
    }
}