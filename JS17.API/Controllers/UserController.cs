using AutoMapper;
using JS17.API.Models.Dtos;
using JS17.API.Persistence;
using JS17.API.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace JS17.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly WebDbContext _dbContext;

    public UserController(IMapper mapper, WebDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Register(
        [FromQuery] string name, [FromQuery] string email, [FromQuery] string password)
    {
        if (_dbContext.Users.Any(x => x.Email == email))
        {
            return BadRequest("user with this email already exist");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            Password = Encryptdata(password)
        };
        _dbContext.Users.Add(user);

        var token = GenerateToken(email, password);
        _dbContext.UserTokens.Add(new UserToken
        {
            User = user,
            Token = token
        });
        _dbContext.SaveChanges();

        return Ok(new RegisterResponseDto { Token = token });
    }

    [HttpGet]
    public IActionResult Login([FromQuery] string email, [FromQuery] string password)
    {
        if(!_dbContext.Users.Any(x => x.Email == email))
        {
            return BadRequest("email or password not correct");
        }

        var user = _dbContext.Users.Single(x => x.Email == email);
        var encryptPassword = Encryptdata(password);
        if(encryptPassword != user.Password)
        {
            return BadRequest("email or password not correct");
        }

        var token = GenerateToken(email, password);
        _dbContext.UserTokens.Add(new UserToken
        {
            User = user,
            Token = token
        });
        _dbContext.SaveChanges();

        return Ok(new LoginResponseDto { Token = token });
    }
    
    [HttpGet]
    public IActionResult GetProfile([FromQuery] string token)
    {
        if(!_dbContext.UserTokens.Any(x => x.Token == token))
        {
            return BadRequest("token not valid");
        }

        var user = _dbContext.UserTokens.Include(x => x.User).First(x => x.Token == token).User;
        var userDto = _mapper.Map<UserDto>(user);

        return Ok(userDto);
    }

    public static string GenerateToken(string email, string password)
    {
        return Encryptdata(Encryptdata(email) + Encryptdata(password) + Guid.NewGuid().ToString());
    }

    public static string Encryptdata(string password)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }
}
