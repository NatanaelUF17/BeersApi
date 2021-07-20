using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BeersApi.Data;
using BeersApi.DTO;
using BeersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BeersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration { get; set; }
        public BeersContext _context { get; set; }

        public AuthController(IConfiguration configuration, BeersContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        string CreateToken(User user)
        {
            var jwtConfig = _configuration.GetSection("JWT");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Token")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var isssuer = jwtConfig.GetValue<string>("Issuer");
            var audience = jwtConfig.GetValue<string>("Audience");
            var jwtValidity = DateTime.Now.AddMinutes(jwtConfig.GetValue<int>("ExpirationHours"));

            var token = new JwtSecurityToken(
              issuer: isssuer,
              audience: audience,
              claims: new List<Claim>() {
                  new Claim("id", user.Id.ToString()),
                  new Claim("username", user.Username),
              },
              expires: jwtValidity,
              signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("/Login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDTO user)
        {
            var foundUser = await _context
                .Users
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();       
            
            if (foundUser == null)
            {
                return NotFound(new { message = "User Not Found 😭" });
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password);

            if (isValidPassword)
            {
                return Ok(new { token = CreateToken(foundUser) });
            }

            return BadRequest(new { message = "Bad Credentials" });

        }
    }
}