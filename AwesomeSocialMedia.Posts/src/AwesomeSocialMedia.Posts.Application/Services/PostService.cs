using AwesomeSocialMedia.Posts.Application.InputModels;
using AwesomeSocialMedia.Posts.Application.ViewModels;
using AwesomeSocialMedia.Posts.Core.Repositories;
using AwesomeSocialMedia.Posts.Infrastructure.EventBus;
using AwesomeSocialMedia.Posts.Infrastructure.Integration.Services;

namespace AwesomeSocialMedia.Posts.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;
    private readonly IEventBus _bus;
    private readonly IUserIntegrationService _userService;

    public PostService(IPostRepository repository, IEventBus bus, IUserIntegrationService userService)
    {
        _repository = repository;
        _bus = bus;
        _userService = userService;
    }

    public async Task<BaseResult<Guid>> Create(CreatePostInputModel model)
    {
        var userResult = await _userService.GetById(model.UserId);

        if (userResult is null || !userResult.Success)
        {
            return new BaseResult<Guid>(Guid.Empty, false, userResult.Message);
        }

        var post = model.ToEntity();

        await _repository.AddAsync(post);

        foreach (var @event in post.Events)
        {
            _bus.Publish(@event);
        }

        return new BaseResult<Guid>(post.Id);
    }

    public async Task<BaseResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);

        return new BaseResult();
    }

    public async Task<BaseResult<List<PostItemViewModel>>> GetAll(Guid userId)
    {
        var posts = await _repository.GetAllAsync(userId);

        var viewModels = posts.Select(p => new PostItemViewModel(p)).ToList();

        return new BaseResult<List<PostItemViewModel>>(viewModels);
    }
}
