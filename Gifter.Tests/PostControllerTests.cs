//using System;
//using Xunit;

//namespace Gifter.Tests
//{
//    public class UnitTest1
//    {
//        [Fact]
//        public void Two_Numbers_Should_Equal()
//        {
//            var num1 = 200;
//            var num2 = 200;

//            Assert.Equal(num1, num2);
//        }
//    }
//}


using Gifter.Controllers;
using Gifter.Models;
using Gifter.Tests.Mocks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Gifter.Tests
{
    public class PostControllerTests
    {
        //The Get_Returns_All_Posts() method is a single test. Tests are public methods marked with the [Fact] attribute. The [Fact] attribute is provided by the xUnit testing library.
        [Fact]
        public void Get_Returns_All_Posts() //FOCUS ON THIS RIGHT NOW---------------------------------
        {
            // Arrange 
            var postCount = 20;
            var posts = CreateTestPosts(postCount); //create some test Posts,

            var repo = new InMemoryPostRepository(posts); //mock repo
            var controller = new PostController(repo); //instance of the PostController class.

            // Act 
            var result = controller.Get(); //execute the method that is being tested, "PostController.Get() method."

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            //use Assert utility class (xUnit provides)
            //verify the result of Get() method is an instance of the OkObjectResult class
            // this is the type returned by the Ok() method: 
            var actualPosts = Assert.IsType<List<Post>>(okResult.Value);

            //verify that it contains the expected list of posts.
            Assert.Equal(postCount, actualPosts.Count);
            Assert.Equal(posts, actualPosts);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var posts = new List<Post>(); // no posts

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_By_Id_Returns_Post_With_Given_Id()
        {
            // Arrange
            var testPostId = 99; 
            var posts = CreateTestPosts(5);
            posts[0].Id = testPostId; // Make sure we know the Id of one of the posts

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            // Act
            var result = controller.Get(testPostId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPost = Assert.IsType<Post>(okResult.Value);

            Assert.Equal(testPostId, actualPost.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_Post()
        {
            // Arrange 
            var postCount = 20;
            var posts = CreateTestPosts(postCount);

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            // Act
            var newPost = new Post()
            {
                Caption = "Caption",
                Title = "Title",
                ImageUrl = "http://post.image.url/",
                DateCreated = DateTime.Today,
                UserProfileId = 999,
                UserProfile = CreateTestUserProfile(999),
            };

            controller.Post(newPost); //POST method

            // Assert
            Assert.Equal(postCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Returns_BadRequest_When_Ids_Do_Not_Match()
        {
            // Arrange
            var testPostId = 99;
            var posts = CreateTestPosts(5);
            posts[0].Id = testPostId; // Make sure we know the Id of one of the posts

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            var postToUpdate = new Post()
            {
                Id = testPostId,
                Caption = "Updated!",
                Title = "Updated!",
                UserProfileId = 99,
                DateCreated = DateTime.Today,
                ImageUrl = "http://some.image.url",
            };
            var someOtherPostId = testPostId + 1; // make sure they aren't the same

            // Act
            var result = controller.Put(someOtherPostId, postToUpdate);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_Method_Updates_A_Post()
        {
            // Arrange
            var testPostId = 99;
            var posts = CreateTestPosts(5);
            posts[0].Id = testPostId; // Make sure we know the Id of one of the posts

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            var postToUpdate = new Post()
            {
                Id = testPostId,
                Caption = "Updated!",
                Title = "Updated!",
                UserProfileId = 99,
                DateCreated = DateTime.Today,
                ImageUrl = "http://some.image.url",
            };

            // Act
            controller.Put(testPostId, postToUpdate);

            // Assert
            var postFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testPostId);
            Assert.NotNull(postFromDb);

            Assert.Equal(postToUpdate.Caption, postFromDb.Caption);
            Assert.Equal(postToUpdate.Title, postFromDb.Title);
            Assert.Equal(postToUpdate.UserProfileId, postFromDb.UserProfileId);
            Assert.Equal(postToUpdate.DateCreated, postFromDb.DateCreated);
            Assert.Equal(postToUpdate.ImageUrl, postFromDb.ImageUrl);
        }

        [Fact]
        public void Delete_Method_Removes_A_Post()
        {
            // Arrange
            var testPostId = 99;
            var posts = CreateTestPosts(5);
            posts[0].Id = testPostId; // Make sure we know the Id of one of the posts

            var repo = new InMemoryPostRepository(posts);
            var controller = new PostController(repo);

            // Act
            controller.Delete(testPostId);

            // Assert
            var postFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testPostId);
            Assert.Null(postFromDb);
        }

        private List<Post> CreateTestPosts(int count)
        {
            var posts = new List<Post>();
            for (var i = 1; i <= count; i++)
            {
                posts.Add(new Post()
                {
                    Id = i,
                    Caption = $"Caption {i}",
                    Title = $"Title {i}",
                    ImageUrl = $"http://post.image.url/{i}",
                    DateCreated = DateTime.Today.AddDays(-i),
                    UserProfileId = i,
                    UserProfile = CreateTestUserProfile(i),
                });
            }
            return posts;
        }

        private UserProfile CreateTestUserProfile(int id)
        {
            return new UserProfile()
            {
                Id = id,
                Name = $"User {id}",
                Email = $"user{id}@example.com",
                Bio = $"Bio {id}",
                DateCreated = DateTime.Today.AddDays(-id),
                ImageUrl = $"http://user.image.url/{id}",
            };
        }

        //Create a test for the PostController.Search() method.
        [Fact]
        public void Search_Returns_Correct_Posts()
        {
            // Arrange
            // Set the number of posts to create for testing.
            var postCount = 20;

            // Create list of test posts using the CreateTestPosts method.
            // This generate a list of 'postCount' number of posts with various captions.
            var posts = CreateTestPosts(postCount);

            // Create an instance of InMemoryPostRepository with the generated list of posts.
            // This simulates a repository where the tests posts are stored in memory for testing purposes.This simulates a real database.
            var repo = new InMemoryPostRepository(posts);

            // Create an instance of PostController, passing the mock repository as a parameter.
            // This allows us to test the controller’s behavior without needing a real database.
            var controller = new PostController(repo);

            // Act
            // Call Search method on the controller with a search criterion ("Caption 1").
            // This simulates a client request to search for posts with the given caption.
            var result = controller.Search("Caption 1", true); //note add true for argument for bool sortDescending

            // Assert "Check"
            // Check result of Search is type OkObjectResult, which means the search was successful (HTTP 200 response with a payload)
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Get list of posts from the OkObjectResult's value.
            // Assert that the value is of type List<Post>, confirming the controller returned a list of posts.
            // Get the list of posts from the result and check that it's indeed a list of Post objects.
            var actualPosts = Assert.IsType<List<Post>>(okResult.Value);

            // Assert that the actual list of posts contains exactly one post.
            // This verifies that the search returned the expected number of results.
            // Verify that the result contains exactly one post.
            Assert.Single(actualPosts);

            // Assert that the caption of the returned post matches the search criterion "Caption 1".
            // This ensures that the search functionality is correctly filtering posts based on their captions.
            // Check that the caption of the post matches "Caption 1".
            Assert.Equal("Caption 1", actualPosts[0].Caption);
        }


    }
}