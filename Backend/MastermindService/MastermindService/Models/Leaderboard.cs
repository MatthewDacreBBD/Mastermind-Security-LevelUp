namespace MastermindService.Models
{
    public class Leaderboard
    {

        public Leaderboard(Game game, string username)
        {
            UserGame = game;
            Username = username;
        }

        public Game UserGame { get; set; }

        public String Username { get; set; }
    }
}
