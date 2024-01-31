namespace AwesomeSocialMedia.Newsfeed.Core.Core.Entities;

public class Post
{
    public Post(Guid id, string content, DateTime publishedAt)
    {
        Id = id;
        Content = content;
        PublishedAt = publishedAt;
    }

    public Post()
    {
        
    }

    public Guid Id { get; private set; }
    public string Content { get; private set; }
    public DateTime PublishedAt { get; private set; }
    public Guid UserNewsfeedId { get; set; }
}
