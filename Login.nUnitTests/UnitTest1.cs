using NUnit.Framework;
using aspPro2.Controllers;
using Microsoft.AspNetCore.Mvc;
using aspPro2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Text;
using Humanizer;


// We're testing the SearchController — specifically the Search POST action — to make sure it:
//Returns the correct game if the name exists.
//Returns NotFound if the game doesn't exist or if the search input is null.

// We're also testing the LoginController to see if it retruns the correct view 
namespace Login.nUnitTests
{
    public class LoginControllerTests
    {
        private LoginController LO_controller = null!;
        private ApplicationDbContext _context;
        private SearchController _controller;

    //Setup Method: (Runs before each test)
    [SetUp]
        public void Setup()
        {
            LO_controller = new LoginController(null!);

            //This sets up an in-memory database, so we don’t touch the real database.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDb")
               .Options;

            //ApplicationDbContext is created with the options so we
            //can use EF Core as usual in tests.
            _context = new ApplicationDbContext(options);

            // Seed test data
            //Here, we insert two sample game records into the database,
            //so we can test searching for them.
            _context.gameInfo.Add(new Games { Id = 1, Name = "Elden Ring" });
            _context.gameInfo.Add(new Games { Id = 2, Name = "dark souls" });
            _context.SaveChanges();


            //We create the controller and pass in our fake (in-memory) _context
            _controller = new SearchController(_context);

        }
        [TearDown]
        public void TearDown()
        {
            //This cleans up the database after each test to ensure that
            //every test runs with a clean state.
            LO_controller?.Dispose();

            _context.Database.EnsureDeleted();
            _context.Dispose();
            _controller.Dispose();
        }

        [Test]
        public void SingUp_Retunrs_VeiwResult()
        {
            // Act
            var result = LO_controller.signUp() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsNull(result!.ViewName);
        }


        [Test]
        public async Task Search_ReturnsGame_WhenNameMatches()
        {
            //this test calls the Search method with "Elden Ring"
            //and make sure "Asserts that ""
            // A ViewResult was retruned 
            // The model passed to the view is a Games class object
            // The Name property matches "Elden Ring"

            // Act
            var result = await _controller.Search("Elden Ring") as ViewResult;

            // Assert
            // Make sure we got a view
            Assert.IsNotNull(result);
            // The model should be a Games object
            Assert.IsInstanceOf<Games>(result.Model);
            var game = result.Model as Games;
            // Check if it returned the correct game
            Assert.AreEqual("Elden Ring", game.Name);
        }
        

        [Test]
        public async Task Search_ReturnsNotFound_WhenGameNotExists()
        {
            // this test calls the Search method with "Nonexistent Game" a Name that 
            // does not exist in the DB 
            // and make sure "Asserts that" : the result is a NotFoundResult

            // Act
            var result = await _controller.Search("Nonexistent Game");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Search_ReturnsNotFound_WhenSearchIsNull()
        {
            // this test calls the Search method with a null value
            // the same as the test above expects a NotFoundResult

            // Act
            var result = await _controller.Search(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }



    }
}