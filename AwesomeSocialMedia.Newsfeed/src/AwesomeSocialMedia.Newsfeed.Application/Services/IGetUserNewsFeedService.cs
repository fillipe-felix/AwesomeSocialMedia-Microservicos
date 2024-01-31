using AwesomeSocialMedia.Newsfeed.Core.Core.Entities;

namespace AwesomeSocialMedia.Newsfeed.Application.Services;

public interface IGetUserNewsFeedService
{
    Task<UserNewsfeed> GetUserNewsFeed(Guid userId);
}
