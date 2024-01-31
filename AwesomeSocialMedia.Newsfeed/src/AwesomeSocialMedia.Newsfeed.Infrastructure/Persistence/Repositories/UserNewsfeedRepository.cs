using AwesomeSocialMedia.Newsfeed.Core.Core.Entities;
using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence.Repositories;

public class UserNewsfeedRepository : IUserNewsfeedRepository
{
    private readonly NewsFeedDbContext _context;

    public UserNewsfeedRepository(NewsFeedDbContext context)
    {
        _context = context;
    }


    public async Task AddPost(User user, Post post)
    {
        var newsfeed = await _context.UserNewsfeeds
            .Include(p => p.Posts)
            .FirstOrDefaultAsync(n => n.User.Id == user.Id);

        if (newsfeed is null)
        {
            newsfeed = new UserNewsfeed(new List<Post>(), user);
            post.UserNewsfeedId = newsfeed.Id;
            await _context.Posts.AddAsync(post);
        }

        newsfeed.AddPost(post);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task<UserNewsfeed?> GetByUserId(Guid userId)
    {
        return await _context.UserNewsfeeds
            .Include(p => p.Posts)
            .FirstOrDefaultAsync(n => n.User.Id == userId);
    }
    
    public async Task Updated(UserNewsfeed userNewsfeed)
    {
        _context.UserNewsfeeds.Update(userNewsfeed);
        await _context.SaveChangesAsync();
    }
}
