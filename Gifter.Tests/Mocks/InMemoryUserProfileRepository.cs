//Create tests for all the basic CRUD functionality in the UserProfileController. You should create at least one test for each of these controller methods.

//using Gifter.Models;

//Get()
//Get(int id)
//Post(UserProfile userProfile)
//Put(int id, UserProfile userProfile)
//Delete(int id)
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using Gifter.Models;
using Gifter.Repositories;

namespace Gifter.Tests.Mocks
{
     class InMemoryUserProfileRepository: IUserProfileRepository
    {

        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public void Add(UserProfile userProfile)
        {
            var lastUserProfile = _data.Last();
            userProfile.Id = lastUserProfile.Id + 1;
            _data.Add(userProfile);
        }

        public void Delete(int id)
        {
            var userProfileTodelete = _data.FirstOrDefault(p => p.Id == id);
            if (userProfileTodelete == null)
            {
                return;
            }

            _data.Remove(userProfileTodelete);
        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public UserProfile GetByIdWithPosts(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile userProfile)
        {
            var currentUserProfile = _data.FirstOrDefault(p => p.Id == userProfile.Id);
            if (currentUserProfile == null)
            {
                return;
            }

            currentUserProfile.Name = userProfile.Name;
            currentUserProfile.Email = userProfile.Email;
            currentUserProfile.DateCreated = userProfile.DateCreated;
            currentUserProfile.ImageUrl = userProfile.ImageUrl;
            currentUserProfile.Bio = userProfile.Bio;
        }

        public List<UserProfile> Search(string criterion, bool sortDescending)
        {
            throw new NotImplementedException();
        }

        public List<UserProfile> GetAllWithComments()
        {
            throw new NotImplementedException();
        }

        public UserProfile GetUserProfileByIdWithComments(int id)
        {
            throw new NotImplementedException();
        }
    }

}
