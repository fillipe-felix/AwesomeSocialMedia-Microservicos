namespace AwesomeSocialMedia.Users.Core.Events;

public class UserUpdated : IEvent
{
    public UserUpdated(string displayName)
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; set; }
}
