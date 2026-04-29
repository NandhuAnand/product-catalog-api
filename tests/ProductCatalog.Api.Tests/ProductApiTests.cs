using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ProductCatalog.Api.Tests;

public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_Should_Return_Unauthorized_When_No_Token()
    {
        var response = await _client.GetAsync("/api/v1/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_Should_Return_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            userName = "admin",
            password = "Admin@123"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}