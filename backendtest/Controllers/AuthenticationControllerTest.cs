﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskSync.Controllers;
using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSyncTest.Controllers
{
    public class AuthenticationControllerTest
    {
        [Theory]
        [InlineData("dummy-token", 200)]
        [InlineData(null, 401)]
        public async Task LoginEndpoint_ShouldReturnCorrectStatus(string token, int expectedStatus)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService.Setup(x => x.AuthenticateAsync(It.IsAny<LoginRequest>())).ReturnsAsync(token);
            var authenticationController = new AuthenticationController(mockAuthenticationService.Object);

            // Act
            var request = new LoginRequest() { Email = "email", Password = "password" };
            var response = await authenticationController.Login(request);

            // Assert
            if (expectedStatus == 200)
            {
                var okResult = Assert.IsType<OkObjectResult>(response);
                var loginObj = Assert.IsType<LoginResponse>(okResult.Value);
                Assert.Equal(token, loginObj.Token);
            }
            else if (expectedStatus == 401)
            {
                Assert.IsType<UnauthorizedObjectResult>(response);
            }
        }
    }
}
