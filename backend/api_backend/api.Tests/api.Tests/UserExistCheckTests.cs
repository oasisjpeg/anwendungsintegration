using Moq;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;
using Xunit;

namespace api.Tests
{
    public class UserExistCheckTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserExistCheck _userExistCheck;

        public UserExistCheckTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userExistCheck = new UserExistCheck(_mockUserRepository.Object);
        }

        [Fact]
        public async Task UserExistsAsync_WithExistingEmail_ReturnsTrue()
        {
            // Arrange
            var email = "existing@example.com";
            var user = new UserModel { Name = "test", Email = email, Points = 20};

            _mockUserRepository.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _userExistCheck.UserExistsAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExistsAsync_WithNonExistingEmail_ReturnsFalse()
        {
            // Arrange
            var email = "nonexisting@example.com";

            _mockUserRepository.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync((UserModel)null);

            // Act
            var result = await _userExistCheck.UserExistsAsync(email);

            // Assert
            Assert.False(result);
        }
    }
}