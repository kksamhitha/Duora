using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        if(await EmailExists(registerUserDto.Email)) return BadRequest("Email Already exists.")
;
        var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = registerUserDto.DisplayName,
            Email = registerUserDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
        if(user == null) return Unauthorized("Invalid Email address");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for(var i =0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i]) return  Unauthorized("Invalid Password");
        }

        return user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
