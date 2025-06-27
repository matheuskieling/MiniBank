using System.Net;
using System.Net.Http.Json;
using API.Models;
using API.Models.DTO;
using API.Services;
using API.Services.Interfaces;
using Identity.Models.DTO;
using IntegrationTests.Infra;
using Microsoft.AspNetCore.Http;
using NuGet.Frameworks;

namespace IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class TransactionTests : IDisposable
{
    private readonly ApiIntegrationTestsFixture<ApiProgram> _apiFixture;
    private readonly IdentityIntegrationTestsFixture<IdentityProgram> _identityFixture;
    private readonly DateTime _startTime;
    public TransactionTests()
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
    public async Task ValidTransactionShouldReturnOkAndUpdateWallets()
    {
        await _apiFixture.DepositToWallet(1000);
        var userWallet = await _apiFixture.GetCurrentUserWallet();
        Assert.Equal(1000, userWallet.Balance);
        
        AuthRequest auth = new AuthRequest("test2@tests.com", "test2");
        var receiverWallet = await _identityFixture.CreateUserAndGetWallet(auth);
        Assert.Equal(0, receiverWallet.Balance);

        var request = new TransactionRequestDto(receiverWallet.Id, 500);
        var transaction = await _apiFixture.CreateTransaction(request);
        Assert.NotNull(transaction);
        Assert.Equal(500, transaction.Amount);
        Assert.Equal(userWallet.Id, transaction.SenderId);
        Assert.Equal(receiverWallet.Id, transaction.ReceiverId);
        Assert.InRange(transaction.CreatedAt, _startTime, DateTime.UtcNow);

        var updatedUserWallet = await _apiFixture.GetCurrentUserWallet();
        var updatedReceiverWallet = await _apiFixture.GetWalletById(receiverWallet.Id);
        
        Assert.Equal(500, updatedUserWallet.Balance);
        Assert.Equal(500, updatedReceiverWallet!.Balance);

        var transactions = (await _apiFixture.TransactionService.GetTransactionsBySenderId(transaction.SenderId)).ToList();
        Assert.NotNull(transactions);
        Assert.Single(transactions);
        var transactionFromDb = transactions.Single();
        Assert.Equal(transaction.Id, transactionFromDb.Id);
        Assert.Equal(transaction.Amount, transactionFromDb.Amount);
        Assert.Equal(transaction.CreatedAt, transactionFromDb.CreatedAt);
        Assert.Equal(transaction.SenderId, transactionFromDb.SenderId);
        Assert.Equal(transaction.ReceiverId, transaction.ReceiverId);
    }
    
    [Fact]
    public async Task TransactionWithoutBalanceShouldReturnUnprocessableEntity()
    {
        var userWallet = await _apiFixture.GetCurrentUserWallet();
        Assert.Equal(0, userWallet.Balance);
        
        AuthRequest auth = new AuthRequest("test2@tests.com", "test2");
        var receiverWallet = await _identityFixture.CreateUserAndGetWallet(auth);
        Assert.Equal(0, receiverWallet.Balance);

        var request = new TransactionRequestDto(receiverWallet.Id, 500);
        var transaction = await _apiFixture.CreateTransactionRequest(request);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, transaction.StatusCode);

        var updatedUserWallet = await _apiFixture.GetCurrentUserWallet();
        var updatedReceiverWallet = await _apiFixture.GetWalletById(receiverWallet.Id);
        
        Assert.Equal(0, updatedUserWallet.Balance);
        Assert.Equal(0, updatedReceiverWallet!.Balance);

        var transactions = (await _apiFixture.TransactionService.GetTransactionsBySenderId(userWallet.Id)).ToList();
        Assert.NotNull(transactions);
        Assert.Empty(transactions);
    }
    
    [Fact]
    public async Task TransactionForInexistentUserShouldReturnNotFound()
    {
        await _apiFixture.DepositToWallet(1000);
        var userWallet = await _apiFixture.GetCurrentUserWallet();
        Assert.Equal(1000, userWallet.Balance);
        
        var request = new TransactionRequestDto(Guid.NewGuid(), 500);
        var transaction = await _apiFixture.CreateTransactionRequest(request);
        Assert.Equal(HttpStatusCode.NotFound, transaction.StatusCode);

        var updatedUserWallet = await _apiFixture.GetCurrentUserWallet();
        
        Assert.Equal(1000, updatedUserWallet.Balance);

        var transactions = (await _apiFixture.TransactionService.GetTransactionsBySenderId(userWallet.Id)).ToList();
        Assert.NotNull(transactions);
        Assert.Empty(transactions);
    }
}