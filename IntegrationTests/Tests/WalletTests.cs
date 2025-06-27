using System.Net.Http.Json;
using API.Models;
using IntegrationTests.Infra;

namespace IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class WalletTests : IDisposable
{
    private readonly ApiIntegrationTestsFixture<ApiProgram> _apiFixture;
    private readonly IdentityIntegrationTestsFixture<IdentityProgram> _identityFixture;
    private readonly DateTime _startTime;
    public WalletTests()
    {
        _startTime = DateTime.UtcNow;
        _apiFixture = new ApiIntegrationTestsFixture<ApiProgram>();
        _identityFixture = new IdentityIntegrationTestsFixture<IdentityProgram>();
        _apiFixture.SetTokenForTests(_identityFixture.LoginDefaultUser().Result.Token);
    }
    public void Dispose()
    {
        _apiFixture.Dispose();
        _identityFixture.Dispose();
    }

    [Fact]
    public async Task GetWalletByUserIdShouldReturnWallet()
    {
        var wallet = await _apiFixture.GetWalletByUserId(_identityFixture.DefaultUserId);
        Assert.NotNull(wallet);
        Assert.Equal(_identityFixture.DefaultUserId, wallet.UserId);
        Assert.Equal(0, wallet.Balance);
        Assert.InRange(wallet.CreatedAt, _startTime, DateTime.UtcNow);
    }
    
    [Fact]
    public async Task InsertingFundsReturnsOkAndFundsAreAddedToWallet()
    {
        var depositResponse = await _apiFixture.DepositToWallet(1000);
        depositResponse.EnsureSuccessStatusCode();
        var wallet = await _apiFixture.GetWalletByUserId(_identityFixture.DefaultUserId);
        Assert.NotNull(wallet);
        Assert.Equal(1000, wallet.Balance);
    }
    
    [Fact]
    
    public async Task GetWalletByIdShouldReturnWallet()
    {
        var walletByUserId = await _apiFixture.GetWalletByUserId(_identityFixture.DefaultUserId);
        Assert.NotNull(walletByUserId);
        var walletById = await _apiFixture.GetWalletById(walletByUserId.Id);  
        Assert.NotNull(walletById);
        Assert.Equal(_identityFixture.DefaultUserId, walletById.UserId);
        Assert.Equal(0, walletById.Balance);
        Assert.InRange(walletById.CreatedAt, _startTime, DateTime.UtcNow);
    }
    
}