using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication1.Application_Layer.DTO.User;
using Xunit;

namespace api.IntegrationTests
{
    public class LoginIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public LoginIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            // Arrange
            var email = $"login-test-{Guid.NewGuid()}@example.com";
            var password = "Password123!";
            
            // Register a user first
            var userRegisterDto = new UserRegisterDto
            {
                Name = "Login Test User",
                Email = email,
                Password = password
            };

            var registerContent = new StringContent(
                JsonSerializer.Serialize(userRegisterDto),
                Encoding.UTF8,
                "application/json");

            await _client.PostAsync("/api/users/register", registerContent);

            // Now try to login
            var loginDto = new UserAuthDto
            {
                Email = email,
                Password = password
            };

            var loginContent = new StringContent(
                JsonSerializer.Serialize(loginDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/login", loginContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Login successful");
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new UserAuthDto
            {
                Email = $"nonexistent-{Guid.NewGuid()}@example.com",
                Password = "InvalidPassword123!"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
