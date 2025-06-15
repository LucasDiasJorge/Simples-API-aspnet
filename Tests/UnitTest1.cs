using API.Models;
using Xunit;

// https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022#create-a-project-to-test
// https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
public class UserTests
{
    [Fact]
    public void CreateUser_ShouldInitializeCorrectly()
    {
        // Arrange
        Company company = new Company { Name = "CompanyTest" };

        // Act
        User user = new User
        {
            Name = "UserNameTest",
            Email = "useremail@test.com",
            Password = "nonHashedPass",
            Company = company
        };

        // Assert
        Assert.NotNull(user);
        Assert.Equal("UserNameTest", user.Name);
        Assert.Equal("useremail@test.com", user.Email);
        Assert.Equal("nonHashedPass", user.Password);
        Assert.NotNull(user.Company);
        Assert.Equal("CompanyTest", user.Company.Name);
    }
}