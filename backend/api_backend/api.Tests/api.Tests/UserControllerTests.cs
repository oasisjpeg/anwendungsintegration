using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.API.Controller.User;
using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Application_Layer.Services.Leaderboard;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;
using Xunit;

namespace api.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUserAuth> _mockUserAuth;
        private readonly Mock<IUserExistCheck> _mockUserExistCheck;
        private readonly Mock<ILeaderboardServices> _mockLeaderboardServices;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserAuth = new Mock<IUserAuth>();
            _mockUserExistCheck = new Mock<IUserExistCheck>();
            _mockLeaderboardServices = new Mock<ILeaderboardServices>();

            _controller = new UserController(
                _mockUserRepository.Object,
                _mockUserAuth.Object,
                _mockUserExistCheck.Object,
                _mockLeaderboardServices.Object);
        }

        [Fact]
        public async Task Register_WithNewUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123"
            };

            var createdUser = new UserModel
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Points = 20,
            };

            _mockUserExistCheck.Setup(x => x.UserExistsAsync(userRegisterDto.Email))
                .ReturnsAsync(false);

            _mockUserAuth.Setup(x => x.HashPassword(userRegisterDto.Password))
                .Returns("hashedPassword");

            _mockUserRepository.Setup(x => x.RegisterAsync(userRegisterDto.Name, userRegisterDto.Email, "hashedPassword"))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _controller.Register(userRegisterDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Login", createdAtActionResult.ActionName);
            Assert.Equal(createdUser.Email, ((dynamic)createdAtActionResult.RouteValues).email);
            Assert.Equal(createdUser.Name, ((dynamic)createdAtActionResult.RouteValues).name);
        }
    }
}