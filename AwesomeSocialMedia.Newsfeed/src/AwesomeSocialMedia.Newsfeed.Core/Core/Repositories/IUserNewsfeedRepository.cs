using AwesomeSocialMedia.Newsfeed.Core.Core.Entities;

namespace AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;

public interface IUserNewsfeedRepository
{
    Task AddPost(User user, Post post);
    Task<UserNewsfeed?> GetByUserId(Guid userId);
    Task Updated(UserNewsfeed userNewsfeed);
}
