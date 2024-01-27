using AwesomeSocialMedia.Posts.Infrastructure.Integration.Models;

using Consul;

using Newtonsoft.Json;

namespace AwesomeSocialMedia.Posts.Infrastructure.Integration.Services;

public class UserIntegrationService : IUserIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IConsulClient _consulClient;

    public UserIntegrationService(HttpClient httpClient, IConsulClient consulClient)
    {
        _httpClient = httpClient;
        _consulClient = consulClient;
    }

    public async Task<BaseResult<GetUserByIdViewModel>> GetById(Guid id)
    {
        var queryConsul = await _consulClient.Agent.Services();

        var keyValeuPair = queryConsul.Response.FirstOrDefault(r => r.Value.Service.Equals("Users"));
        var agentService = keyValeuPair.Value;

        var fullUrl = $"http://{agentService.Address}:{agentService.Port}/api/users/{id}";

        var response = await _httpClient.GetAsync(fullUrl);

        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<BaseResult<GetUserByIdViewModel>>(result);
    }
}
