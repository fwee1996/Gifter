using System;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;
using Gifter.Models;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        //add these-----------------------
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        //[HttpGet("GetWithPosts")]
        //public IActionResult GetWithPosts()
        //{
        //    var posts = _postRepository.GetAllWithPosts();
        //    return Ok(posts);
        //}

        //---------------------------------------


        // GET: api/<UserProfileController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userProfileRepository.GetAll());
        }



        [HttpGet("GetUserByIdWithPosts/{id}")]
        public IActionResult GetUserByIdWithPosts(int id)
        {
            var userProfile = _userProfileRepository.GetByIdWithPosts(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        // GET api/<UserProfileController>/5
        //[HttpGet("GetByIdWithPosts")] //I added this-----------------------------------------------------------------
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userProfile = _userProfileRepository.GetByIdWithPosts(id); //----------------chenaged GetById(id) to that
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        // POST api/<UserProfileController>
        [HttpPost]
        public IActionResult Post(UserProfile userProfile)
        {
            _userProfileRepository.Add(userProfile);
            return CreatedAtAction("Get", new { id = userProfile.Id }, userProfile);
        }

        // PUT api/<UserProfileController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _userProfileRepository.Update(userProfile);
            return NoContent();
        }

        // DELETE api/<UserProfileController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userProfileRepository.Delete(id);
            return NoContent();
        }

        //Tommy's:
        //[HttpGet("GetAllUsersWithPosts")]
        //public IActionResult GetAllUsersWithPosts()
        //{
        //    var userProfiles = _userProfileRepository.GetAllUsersWithPosts(); //with post?
        //    return Ok(userProfiles);
        //}

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var userProfiles = _userProfileRepository.GetAll(); //with post?
            return Ok(userProfiles);
        }


    }
}
