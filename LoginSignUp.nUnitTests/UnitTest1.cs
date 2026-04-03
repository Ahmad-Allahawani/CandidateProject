using NUnit.Framework;

namespace LoginSignUp.nUnitTests

{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.CreateUser()).Returns(new User());

            var controller = new LoginController(mockService.Object);
            // Act 
            var result = controller.signUp();

            // Assert
            var viewResult = result as viewResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceof<User>(viewResult.Model);
        }
    }
}