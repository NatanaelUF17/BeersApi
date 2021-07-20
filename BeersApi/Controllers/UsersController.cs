using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeersApi.Data;
using BeersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using BeersApi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace BeersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        public BeersContext _context { get; set; }

        public UsersController(BeersContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var users = await _context
                .Users
                .Select(user => new UserDTO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            });
        }        
    }
}