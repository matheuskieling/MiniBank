using System.Net.Http.Headers;
using System.Net.Http.Json;
using API.Models;
using API.Models.DTO;
using API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infra;

public class ApiIntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
{
    private ApiWebApplicationFactory<TProgram> Factory { get; }
    private HttpClient Client { get; }
    public IWalletService WalletService => Factory.Services.GetRequiredService<IWalletService>();
    public ITransactionService TransactionService => Factory.Services.GetRequiredService<ITransactionService>();
    public ApiIntegrationTestsFixture()
    {
        Factory = new ApiWebApplicationFactory<TProgram>(TestContainerFactory.GetContainer(), "bank");
        Client = Factory.CreateClient();
    }

    public void SetTokenForTests(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
        TestContainerFactory.DisposeContainer();
    }

    public async Task<Wallet?> GetWalletByUserId(Guid userId)
    {
        var response = await Client.GetAsync($"Wallet/User/{userId}");
        var wallet = await response.Content.ReadFromJsonAsync<Wallet>();
        return wallet;
    }
    
    public async Task<Wallet?> GetWalletById(Guid id)
    {
        var response = await Client.GetAsync($"Wallet/{id}");
        var wallet = await response.Content.ReadFromJsonAsync<Wallet>();
        return wallet;
    }

    public async Task<HttpResponseMessage> CreateTransactionRequest(TransactionRequestDto request)
    {
        return await Client.PostAsJsonAsync($"Transaction", request);
    }
    
    public async Task<Transaction> CreateTransaction(TransactionRequestDto request)
    {
        var transactionResponse = await CreateTransactionRequest(request);
        transactionResponse.EnsureSuccessStatusCode();
        var transaction = await transactionResponse.Content.ReadFromJsonAsync<Transaction>();
        Assert.NotNull(transaction);
        return transaction;
    }
    
    public async Task<HttpResponseMessage> DepositToWallet(long amount)
    {
        var request = new AddFundsToWalletRequestDto(amount);
        return await Client.PostAsJsonAsync("Wallet/Deposit", request);
    }

    public async Task<Wallet> GetCurrentUserWallet()
    {
        var request = await Client.GetAsync("Wallet");
        var userWallet = await request.Content.ReadFromJsonAsync<Wallet>();
        Assert.NotNull(userWallet);
        return userWallet;
    }
}