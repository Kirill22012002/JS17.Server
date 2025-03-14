using JS17.API.Persistence;
using JS17.API.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace JS17.API.Controllers;

public class BaseController : ControllerBase
{
    protected WebDbContext _dbContext;

    public BaseController(WebDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected User GetUserByToken(string token)
    {
        if (!_dbContext.UserTokens.Any(x => x.Token == token))
        {
            throw new BadHttpRequestException("token not valid");
        }
        var user = _dbContext.UserTokens.Include(x => x.User).First(x => x.Token == token).User;
        return user;
    }

    protected User GetUserWithPostsByToken(string token)
    {
        if (!_dbContext.UserTokens.Any(x => x.Token == token))
        {
            throw new BadHttpRequestException("token not valid");
        }
        var user = _dbContext.UserTokens.Include(x => x.User).First(x => x.Token == token).User;
        var userWithPosts = _dbContext.Users.Include(x => x.Posts).Single(x => x.Id == user.Id);
        return userWithPosts;
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
