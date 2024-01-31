using AwesomeSocialMedia.Newsfeed.Core.Core.Entities;
using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;

namespace AwesomeSocialMedia.Newsfeed.Application.Services;

public class GetUserNewsFeedService : IGetUserNewsFeedService
{
    private readonly IUserNewsfeedRepository _newsfeedRepository;

    public GetUserNewsFeedService(IUserNewsfeedRepository newsfeedRepository)
    {
        _newsfeedRepository = newsfeedRepository;
    }

    public async Task<UserNewsfeed> GetUserNewsFeed(Guid userId)
    {
        var user = await _newsfeedRepository.GetByUserId(userId);

        return user;
    }
}
