using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{

    [Fact]
    public void AddUser_Should_ReturnFalse_When_FirstName_Is_Missing()
    {
        var userService = new UserService();
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert.Equal(false, addResult);
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_ValidData_ReturnsTrue()
    {
        // Arrange
        var userService = new UserService();
        // Mock ClientRepository and UserCreditService as needed
        
        // Act
        var result = userService.AddUser("John", "Doe", "john@example.com", new DateTime(1990, 1, 1), 1);

        // Assert
        Assert.True(result);
    }
}