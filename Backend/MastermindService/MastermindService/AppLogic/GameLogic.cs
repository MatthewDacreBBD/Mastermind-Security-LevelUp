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

        // getting a list of all users
        public List<Game> getGameList(MastermindDBContext db)
        {
            List<Game> lstgames = new List<Game>();
            var games = from g in db.Game
                        select g;
            foreach (var game in games)
            {
                lstgames.Add(game);
            }
            return lstgames;
        }

        // function to update the user
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

        // function to update the user
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
    }
}