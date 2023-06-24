using MastermindService.AppLogic;
using MastermindService.Data;
using MastermindService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MastermindService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private MastermindDBContext db;
        private GameLogic gameLogic = new GameLogic();

        public GameController(MastermindDBContext db)
        {
            this.db = db;
        }

        // GET: api/<GameController>
        [Authorize]
        [HttpGet]
        public IActionResult GetAllGames()
        {
            List<Game> lstGames = gameLogic.getGameList(db);
            return Ok(lstGames);
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public IActionResult GetSingleGame(int id)
        {
            var game = gameLogic.getGameByID(id, db);
            return Ok(game);
        }

        // POST api/<GameController>
        [HttpPost]
        public void AddGame([FromBody] Game game)
        {
            var response = gameLogic.addGame(game, db);
        }

        // PUT api/<GameController>/5
        [HttpPut]
        public void UpdateGame([FromBody] Game game)
        {
            var response = gameLogic.updateGame(game, db);
        }

        [HttpGet]
        [Route("/api/leaderboard")]
        public IActionResult GetLeaderboard()
        {
            List<Leaderboard> lstLeaderboard = gameLogic.getLeaderboards(db);
            return Ok(lstLeaderboard);
        }
    }
}