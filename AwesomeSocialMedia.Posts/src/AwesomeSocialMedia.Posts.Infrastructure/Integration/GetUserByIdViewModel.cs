﻿namespace AwesomeSocialMedia.Posts.Infrastructure.Integration;

public class GetUserByIdViewModel
{
    public string DisplayName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string? Header { get; private set; }
    public string? Description { get; private set; }
    public string? Country { get; private set; }
    public string? Website { get; private set; }
}
