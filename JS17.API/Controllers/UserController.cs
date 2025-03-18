using AutoMapper;
using JS17.API.Models.Dtos;
using JS17.API.Persistence;
using JS17.API.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace JS17.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : BaseController
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper, WebDbContext dbContext) : base(dbContext)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Register([FromQuery] string name, [FromQuery] string email, [FromQuery] string password)
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
        var user = GetUserByToken(token);
        var userDto = _mapper.Map<UserDto>(user);

        return Ok(userDto);
    }
}
