namespace AwesomeSocialMedia.Newsfeed.Core.Core.Entities;

public class UserNewsfeed
{
    public UserNewsfeed()
    {
        
    }
    public UserNewsfeed(List<Post> posts, User user)
    {
        Id = Guid.NewGuid();
        Posts = posts;
        User = user;
    }
    

    public Guid Id { get; set; }
    public IList<Post> Posts { get; private set; }
    public User User { get; private set; }

    public void AddPost(Post post)
    {
        Posts.Add(post);
    }
}
