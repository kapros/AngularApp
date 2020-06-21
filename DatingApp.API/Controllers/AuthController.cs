using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            //validate request
            username = username.ToLower();
            if (await _authRepository.UserExistsAsync(username))
                return BadRequest("This user already exists!");
            
            var userToCreate = new User
            {
                Username = username
            };

            var createdUser = await _authRepository.RegisterAsync(userToCreate, password);
            
            return StatusCode(201);
        }
    }
}
