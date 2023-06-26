using MastermindGameService.AppLogic;
using MastermindGameService.Data;
using MastermindGameService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MastermindGameService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private MastermindGameDBContext db;
        private readonly IConfiguration _configuration;
        private GameLogic gameLogic = new GameLogic();

        public GameController(MastermindGameDBContext db, IConfiguration configuration)
        {
            this.db = db;
            _configuration = configuration;
        }

        // GET: api/<GameController>
        [Authorize]
        [HttpGet]
        public IActionResult GetAllGames()
        {
            // Retrieve the authorization header
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        // Your existing logic to retrieve the game list
                        List<Game> lstGames = gameLogic.getGameList(db);
                        return Ok(lstGames);
                    }
                    else
                    {
                        // userId is null or empty
                        return BadRequest("Invalid user ID");
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                return Unauthorized();
            }
        }

        // GET api/<GameController>/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetSingleGame(int id)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var game = gameLogic.getGameByID(id, db);
                        return Ok(game);
                    }
                    else
                    {
                        // userId is null or empty
                        return BadRequest("Invalid user ID");
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                return Unauthorized();
            }
        }

        // POST api/<GameController>
        [Authorize]
        [HttpPost]
        public void AddGame([FromBody] Game game)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var response = "";

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        response = gameLogic.addGame(game, db);
                    }
                    else
                    {
                        // userId is null or empty
                        response = "Invalid user ID";
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    response = ex.Message;
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                response = "Unauthorised user";
            }
        }

        // PUT api/<GameController>/5
        [HttpPut]
        public void UpdateGame([FromBody] Game game)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var response = "";

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        response = gameLogic.updateGame(game, db);
                    }
                    else
                    {
                        // userId is null or empty
                        response = "Invalid user ID";
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    response = ex.Message;
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                response = "Unauthorised user";
            }
        }

        //private function to Validate the token
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]))
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                // Token validation failed
                // Handle the exception appropriately or throw a custom exception
                throw new Exception("Invalid token", ex);
            }
        }
    }
}