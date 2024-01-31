using AwesomeSocialMedia.Newsfeed.Core.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence;

public class NewsFeedDbContext : DbContext
{
    public NewsFeedDbContext(DbContextOptions<NewsFeedDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserNewsfeed> UserNewsfeeds { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserNewsfeed>(e =>
        {
            e.HasKey(p => p.Id);

            e.OwnsOne(u => u.User, builder =>
            {
                builder.Property(p => p.Id).HasColumnName("UserId");
                builder.Property(p => p.DisplayName).HasColumnName("DisplayName");
            });
            
            // Relacionamento entre UserNewsfeed e Post
            e.HasMany(unf => unf.Posts)
                .WithOne()
                .HasForeignKey(post => post.UserNewsfeedId)
                .IsRequired();
        });

        builder.Entity<Post>()
            .HasKey(p => p.Id);
        
        
    }
}
