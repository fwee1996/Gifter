using System;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;
using Gifter.Models;
using Azure.Core;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        //private readonly IUserProfileRepository _userProfileRepository;
        public PostController(IPostRepository postRepository)//, IUserProfileRepository userProfileRepository)
        {
            _postRepository = postRepository;
           // _userProfileRepository = userProfileRepository;
        }

        // To get UserProfile object for a particular Post.Assuming we have a UserProfileRepository,
        //we could do something like this in the PostController: BUT NO BECAUSE MORE THAN SINGLE ROUND TRIP- POOR PERFORMANCE
        //instead use single round trip GetAll in PostRepo

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var posts = _postRepository.GetAll();

        //    foreach (var post in posts)
        //    {
        //        post.UserProfile = _userProfileRepository.GetById(post.UserProfileId);
        //    }

        //    return Ok(posts);
        //}
        //i commented above because its bad code-------------------------------------------------------



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_postRepository.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public IActionResult Post(Post post)
        {
            // Handle optional UserProfile and Comments
            //if (post.UserProfile == null)
            //{
            //    post.UserProfile = new UserProfile(); // Provide a default value if needed
            //}
            post.DateCreated = DateTime.Now;
            post.UserProfileId = 1; //hard coded id just for testing, it's not gonna stay like this once log in works and have user
            _postRepository.Add(post);
            return CreatedAtAction("Get", new { id = post.Id }, post);
        }




        [HttpPut("{id}")]
        public IActionResult Put(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _postRepository.Update(post);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _postRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("GetWithComments")]
        public IActionResult GetWithComments()
        {
            var posts = _postRepository.GetAllWithComments();
            return Ok(posts);
        }

        //added this from Tommy's review:
        [HttpGet("GetPostByIdWithComments/{id}")]
        public IActionResult GetPostByIdWithComments(int id)
        {
            var post = _postRepository.GetPostByIdWithComments(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }


        //The above method will respond to a request that looks like this:

        //        https://localhost:5001/api/post/search?q=p&sortDesc=false

        //Notice the URL's route contains search and the URL's query string has values for q and sortDesc keys.search corresponds to the the argument passed to the [HttpGet("search")] attribute, and q and sortDesc correspond to the method's parameters.
        [HttpGet("search")]
        public IActionResult Search(string q, bool sortDesc)
        {
            return Ok(_postRepository.Search(q, sortDesc));
        }


        ////for: Add a new endpoint, /api/post/hottest?since=<SOME_DATE> that will return posts created on or after the provided date.
        /////add the new /api/post/hottest endpoint
        //[HttpGet("hottest")]
        //public IActionResult GetHottestPosts([FromQuery] DateTime since)
        //{
        //    var posts = _postRepository.GetPostsSince(since);
        //    return Ok(posts);
        //}


        //Key Points:
        //Parameter Handling:

//The since parameter is being passed as a query string in DateTime format(e.g., 2024-09-01). Ensure that the date is formatted properly when making the request.
//        SQL Query:

//The SQL query checks posts where the DateCreated field is on or after the provided date (p.DateCreated >= @Since). This will ensure that you retrieve the "hottest" posts based on the date.
//Sorting:

//The posts are ordered by DateCreated DESC, so the newest posts appear first.

        ////TEST WITH SWAGGER:
        ////api/post/hottest?since=2024-09-01
    }
}