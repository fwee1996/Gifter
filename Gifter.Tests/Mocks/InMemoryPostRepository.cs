////et's focus on the Get() method. What does it do? It returns an object - some kind of IActionResult - that contains a list of all the posts in our database.

//What do you think we'd need to do in order to test the Get() method? Well, to see if the Get() method does what we think it should do, we could run it and see if it does what we think it should do.

//How do we do that?

//Create an instance of the PostController.
//Call the Get() method.
//Check the object returned by the Get() method to see if it contains the list of posts.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Gifter.Controllers;
using Gifter.Models;
using Gifter.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Gifter.Tests.Mocks
{
    //Mocking" the IPostRepository
    //unit test verify single method 
    //how to test a single controller method if each method calls into repo and repo sends SQL commands to database: >one method.
    //so create mock repo for test.Instead of connecting to SQL Server, this mock repo mimic the behavior using a simple list of posts.
    class InMemoryPostRepository : IPostRepository //fake repo mimic IPostRepository with al the same methods in post repo
                                                   //IPostRepository makes our code flexible to use mock repo!Also instead of SQL database, mock repo store data in _data list.
    {
        private readonly List<Post> _data;

        public List<Post> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryPostRepository(List<Post> startingData)
        {
            _data = startingData;
        }

        public void Add(Post post)
        {
            var lastPost = _data.Last();
            post.Id = lastPost.Id + 1;
            _data.Add(post);
        }

        public void Delete(int id)
        {
            var postTodelete = _data.FirstOrDefault(p => p.Id == id);
            if (postTodelete == null)
            {
                return;
            }

            _data.Remove(postTodelete);
        }

        public List<Post> GetAll()
        {
            return _data;
        }

        public Post GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Post post)
        {
            var currentPost = _data.FirstOrDefault(p => p.Id == post.Id);
            if (currentPost == null)
            {
                return;
            }

            currentPost.Caption = post.Caption;
            currentPost.Title = post.Title;
            currentPost.DateCreated = post.DateCreated;
            currentPost.ImageUrl = post.ImageUrl;
            currentPost.UserProfileId = post.UserProfileId;
        }

        public List<Post> Search(string criterion, bool sortDescending)
        {
            var query = _data.Where(p => p.Caption.Contains(criterion, StringComparison.OrdinalIgnoreCase)).ToList();

            // Sort if necessary
            if (sortDescending)
            {
                query = query.OrderByDescending(p => p.DateCreated).ToList();
            }
            else
            {
                query = query.OrderBy(p => p.DateCreated).ToList();
            }

            return query;
        }

        public List<Post> GetAllWithComments()
        {
            throw new NotImplementedException();
        }

        public Post GetPostByIdWithComments(int id)
        {
            throw new NotImplementedException();
        }

      
       
    }
}