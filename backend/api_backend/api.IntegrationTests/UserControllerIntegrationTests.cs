using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication1.Application_Layer.DTO.User;
using Xunit;

namespace api.IntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidUser_ReturnsCreated()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Name = "Integration Test User",
                Email = $"integration-test-{Guid.NewGuid()}@example.com",
                Password = "Password123!"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(userRegisterDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var email = $"integration-test-duplicate-{Guid.NewGuid()}@example.com";
            
            var firstUser = new UserRegisterDto
            {
                Name = "First User",
                Email = email,
                Password = "Password123!"
            };

            var secondUser = new UserRegisterDto
            {
                Name = "Second User",
                Email = email, // Same email as first user
                Password = "Password456!"
            };

            var firstContent = new StringContent(
                JsonSerializer.Serialize(firstUser),
                Encoding.UTF8,
                "application/json");

            var secondContent = new StringContent(
                JsonSerializer.Serialize(secondUser),
                Encoding.UTF8,
                "application/json");

            // Act
            await _client.PostAsync("/api/users/register", firstContent);
            var response = await _client.PostAsync("/api/users/register", secondContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
