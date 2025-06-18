using API.Models;

namespace API.Services.Interfaces;

public interface ICurrentUserService
{
    CurrentUser? CurrentUser { get; set; }
}