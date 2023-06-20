using MastermindService.AppLogic;
using MastermindService.Data;
using MastermindService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MastermindService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MastermindDBContext db;
        private readonly IConfiguration _configuration;
        private UserLogic userLogic = new UserLogic();

        public UserController(MastermindDBContext db, IConfiguration configuration)
        {
            this.db = db;
            _configuration = configuration;
        }

        // GET: api/<UserController>
        //get All users
        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = userLogic.getUserList(db);
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return BadRequest("No Users Present");
            }
        }

        //get user by ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = userLogic.getUserByID(id, db);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("No User Found");
            }
        }

        // function to register a user
        [HttpPost]
        public IActionResult RegisterUser([FromBody] User user)
        {
            var result = userLogic.registerUser(user, db);
            return Ok(result);
        }

        // function to update user details
        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            string updateResult = userLogic.updateUser(user, db);
            return Ok(updateResult);
        }

        // login function with token accesss
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser([FromBody] User user)
        {
            if (user != null)
            {
                var resultLogin = db.User.Where(u => u.username == user.username && u.password == user.password).FirstOrDefault();
                if (resultLogin == null)
                {
                    return BadRequest("Invalid Credentials");
                }
                else
                {
                    resultLogin.authenticationToken = GetToken(resultLogin);
                    return Ok(resultLogin);
                }
            }
            else
            {
                return BadRequest("User Not Found");
            }
        }

        // helper method to generate the token {can add claims and roles}
        private string GetToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}