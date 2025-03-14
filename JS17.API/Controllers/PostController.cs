using AutoMapper;
using JS17.API.Models.Dtos;
using JS17.API.Persistence;
using JS17.API.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JS17.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PostController : BaseController
{
    private readonly IMapper _mapper;

    public PostController(IMapper mapper, WebDbContext dbContext) : base(dbContext)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult AddUserPost([FromQuery] string token, [FromQuery] string title, [FromQuery] string text)
    {
        var user = GetUserByToken(token);
        var post = new Post
        {
            Title = title,
            Text = text,
            Owner = user
        };
        _dbContext.Posts.Add(post);
        _dbContext.SaveChanges();
        return Ok(post.Id);
    }

    [HttpGet]
    public IActionResult UpdateUserPost([FromQuery] string token, [FromQuery] int postId, [FromQuery] string title = "", [FromQuery] string text = "")
    {
        var user = GetUserWithPostsByToken(token);
        var post = user.Posts.Single(x => x.Id == postId);
        if (title.IsNullOrEmpty())
        {
            post.Title = title;
        }
        else if (text.IsNullOrEmpty())
        {
            post.Text = text;
        }
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpGet]
    public IActionResult RemoveUserPost([FromQuery] string token, [FromQuery] int postId)
    {
        var user = GetUserWithPostsByToken(token);
        var post = user.Posts.Single(x => x.Id == postId);
        post.IsRemoved = true;
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpGet]
    public IActionResult GetUserPosts([FromQuery] string token, [FromQuery] int skip, [FromQuery] int take)
    {
        var user = GetUserByToken(token);
        var posts = _dbContext.Posts
            .Include(x => x.Owner)
            .Where(x => x.Owner == user && x.IsRemoved == false)
            .Skip(skip)
            .Take(take)
            .ToList();

        var postDtos = _mapper.Map<List<PostDto>>(posts);
        return Ok(postDtos);
    }

    [HttpGet]
    public IActionResult GetPostById([FromQuery] int postId)
    {
        var post = _dbContext.Posts.Single(x => x.Id == postId);
        var postDto = _mapper.Map<PostDto>(post);
        return Ok(postDto);
    }
}
