namespace MastermindService.Models
{
    public class AppUserDTO
    {
        public AppUserDTO(int userId, string username)
        {
            this.UserId = userId;
            this.UserName = username;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}