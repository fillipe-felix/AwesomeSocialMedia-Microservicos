using AwesomeSocialMedia.Newsfeed.Application.Services;
using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace AwesomeSocialMedia.Newsfeed.API.Controllers;

[Route("api/newsfeed")]
public class NewsfeedsController : Controller
{
    private readonly IGetUserNewsFeedService _getUserNewsFeedService;

    public NewsfeedsController(IGetUserNewsFeedService getUserNewsFeedService)
    {
        _getUserNewsFeedService = getUserNewsFeedService;
    }


    // Não é para pegar a timeline, e sim o feed de posts de um usuário
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserNewsfeed(Guid userId)
    {
        var newsfeed = await _getUserNewsFeedService.GetUserNewsFeed(userId);

        if (newsfeed is null)
        {
            return NotFound();
        }

        return Ok(newsfeed);
    }
}
