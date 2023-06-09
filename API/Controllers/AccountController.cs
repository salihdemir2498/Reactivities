﻿using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (result)
            {
                return new UserDTO
                {
                   DisplayName = user.DisplayName,
                   UserName= user.UserName,
                   Image = null,
                   Token = _tokenService.CreateToken(user)
                };
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDTO.UserName))
            {
                return BadRequest("Username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            //Bu kod parçası, _userManager örneği kullanılarak User nesnesinden (ClaimsPrincipal) kullanıcının e-posta adresini alır ve bu e-posta adresiyle _userManager üzerinden kullanıcıyı bulur.
            //User.FindFirstValue(ClaimTypes.Email) ifadesi, User nesnesinin içerdiği Claims koleksiyonunda ClaimTypes.Email türünde olan ilk iddiayı bulur ve değerini döndürür.Bu durumda, kullanıcının e - posta adresini alır.

            return CreateUserObject(user);
            
        }

        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Image = null
            };
        }
    }
}
