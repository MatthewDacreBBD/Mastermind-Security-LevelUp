namespace MastermindService.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string? gameStatus { get; set; }
        public int score { get; set; }
    }
}