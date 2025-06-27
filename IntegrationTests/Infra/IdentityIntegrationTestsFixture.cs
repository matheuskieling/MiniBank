using System.Net.Http.Json;
using API.Models;
using API.Services.Interfaces;
using Identity.Models.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infra;

public class IdentityIntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
{
    private IdentityWebApplicationFactory<TProgram> Factory { get; }
    private HttpClient Client { get; }
    private AuthRequest AuthRequest => new AuthRequest("testUser@test.com", "testPassword");
    public IWalletService WalletService => Factory.Services.GetRequiredService<IWalletService>();
    public Guid DefaultUserId;
    
    public IdentityIntegrationTestsFixture()
    {
        Factory = new IdentityWebApplicationFactory<TProgram>();
        Client = Factory.CreateClient();
        CreateDefaultUser().Wait();
        DefaultUserId = LoginDefaultUser().Result.Id;
    }

    public async Task CreateDefaultUser()
    {
        var response = await Client.PostAsJsonAsync("Auth/Register", AuthRequest);
    }
    
    public async Task<LoginResponse> LoginDefaultUser()
    {
        var response = await Client.PostAsJsonAsync("Auth/Login", AuthRequest);
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResponse);
        Assert.NotEmpty(loginResponse.Token);
        return loginResponse;
    }
    
    public async Task CreateUser(AuthRequest request)
    {
        var response = await Client.PostAsJsonAsync("Auth/Register", request);
    }

    public async Task<LoginResponse> Login(AuthRequest request)
    {
        var response = await Client.PostAsJsonAsync("Auth/Login", request);
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResponse);
        Assert.NotEmpty(loginResponse.Token);
        return loginResponse;
    }

    public async Task<Wallet> CreateUserAndGetWallet(AuthRequest request)
    {
        await CreateUser(request);
        var receiverUser = await Login(request);
        var receiverWallet = await WalletService.CreateWallet(receiverUser.Id);
        Assert.NotNull(receiverWallet);
        Assert.NotNull(receiverWallet.Data);
        return receiverWallet.Data;
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
        TestContainerFactory.DisposeContainer();
    }
}