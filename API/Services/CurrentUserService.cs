using API.Models;

namespace API.Services.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUser? CurrentUser { get; set; }
}