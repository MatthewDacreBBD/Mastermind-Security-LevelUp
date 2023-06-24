using MastermindService.Data;
using MastermindService.Models;

namespace MastermindService.AppLogic
{
    public class GameLogic
    {
        public Game getGameByID(int id, MastermindDBContext db)
        {
            var game = db.Game.Find(id);
            if (game != null)
            {
                return game;
            }
            else
            {
                return new Game { Id = -1 };
            }
        }

        // getting a list of all games
        public List<Game> getGameList(MastermindDBContext db)
        {
            List<Game> lstgames = db.Game.ToList();
            return lstgames;
        }

        // function to add a game
        public string addGame(Game game, MastermindDBContext db)
        {
            if (game != null)
            {
                var existingGame = db.Game.Find(game.Id);
                if (existingGame != null)
                {
                    return "Game already exists in database";
                }
                else
                {
                    db.Game.Add(game);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                    }

                    return "Game added Successfully";
                }
            }
            else
            {
                return "Game details are invalid";
            }
        }

        // function to update the game
        public string updateGame(Game game, MastermindDBContext db)
        {
            if (game != null)
            {
                var existingGame = db.Game.Find(game.Id);
                if (existingGame != null)
                {
                    existingGame.Id = game.Id;
                    existingGame.userId = game.userId;
                    existingGame.gameStatus = game.gameStatus;
                    existingGame.score = game.score;
                    try
                    {
                        db.SaveChanges();
                        return "Game details updated Successfully";
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                        return "Something went wrong while updating";
                    }
                }
                else
                {
                    return "Could not update game details";
                }
            }
            else
            {
                return "Details provided are null";
            }
        }

        public List<Leaderboard> getLeaderboards(MastermindDBContext db)
        {
            List<Leaderboard> lstLeaderboard = new List<Leaderboard>();
            List<AppUser> lstUsers = db.AppUser.ToList();
            List<Game> lstGames = db.Game.ToList();

            foreach (var user in lstUsers)
            {
                foreach (var game in lstGames)
                {
                    if (user.Id == game.userId)
                    {
                        lstLeaderboard.Add(new Leaderboard(game, user.username));
                    }
                }
            }
            return lstLeaderboard;
        }
    }
}