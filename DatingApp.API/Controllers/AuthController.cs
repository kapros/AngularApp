using DatingApp.API.Data;
using DatingApp.API.DTO;
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
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.Username = userForRegister.Username.ToLower();
            if (await _authRepository.UserExistsAsync(userForRegister.Username))
                return BadRequest("This user already exists!");
            
            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var createdUser = await _authRepository.RegisterAsync(userToCreate, userForRegister.Password);
            
            return StatusCode(201);
        }
    }
}
