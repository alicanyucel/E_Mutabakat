﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Mutabakat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userforRegister)
        { 
            var userExits = _authService.UserExists(userforRegister.Name);
            if(!userExits.Success)
            {
                return BadRequest(userExits.Message);
            }
            var registerResult = _authService.Register(userforRegister, userforRegister.Password);
            return BadRequest(registerResult.Message);
        }
        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            var userToLogin = _authService.Login(userForLogin);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            return Ok(userToLogin);
        }
    }
}
