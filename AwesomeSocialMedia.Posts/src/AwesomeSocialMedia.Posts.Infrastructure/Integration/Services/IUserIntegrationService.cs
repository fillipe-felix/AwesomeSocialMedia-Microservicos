using AwesomeSocialMedia.Posts.Infrastructure.Integration.Models;

namespace AwesomeSocialMedia.Posts.Infrastructure.Integration.Services;

public interface IUserIntegrationService
{
    Task<BaseResult<GetUserByIdViewModel>> GetById(Guid id);
}
