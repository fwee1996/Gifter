using Gifter.Controllers;
using Gifter.Models;
using Gifter.Utils;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll() //http://localhost:5173/api/UserProfile
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT up.Id, up.Name, up.Bio, up.Email, up.DateCreated, 
                       up.ImageUrl 
                       
                  FROM UserProfile up 
                    ";

                    var reader = cmd.ExecuteReader();

                    var userProfiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),

                        });
                    }

                    reader.Close();

                    return userProfiles;
                }
            }
        }













        //get single UserProfile by its id //http://localhost:5173/api/UserProfile/1
        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT 
                                 u.Id, u.Name, u.Email, u.ImageUrl , u.DateCreated, u.Bio 
                                    
                           FROM UserProfile u
                           
                           WHERE u.Id = @Id";
                    //added p.Id in SELECT
                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;
                    if (reader.Read())
                    {

                        userProfile = new UserProfile()
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                        };


                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        //i uncommented above-------------------------------------------------------








        //Add methods to the UserProfileController and UserProfileRepository to return a single UserProfile along with the list of posts authored by that user.  Extra: Plus all the comments for each post.
        //get single UserProfile by its id
        public UserProfile GetByIdWithPosts(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT 
                                 u.Id, u.Name, u.Email, u.ImageUrl , u.DateCreated, u.Bio, 
                                    p.Id AS postId, p.Title, p.Caption, p.DateCreated AS postDateCreated, p.ImageUrl AS postImageUrl, p.UserProfileId 

                           FROM UserProfile u
                           JOIN Post p ON p.UserProfileId=u.Id

                           WHERE u.Id = @Id";
                    //added p.Id in SELECT
                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;


                    while (reader.Read())
                    {
                        if (userProfile == null)
                        {
                            userProfile = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                Posts = new List<Post>()

                            };
                        }
                        if (!DbUtils.IsDbNull(reader, "postId"))
                        {
                            Post post = new Post()
                            {
                                Id = DbUtils.GetInt(reader, "postId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                DateCreated = DbUtils.GetDateTime(reader, "postDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "postImageUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            };

                            // Add the comment to the post's Comments list
                            userProfile.Posts.Add(post);
                        }
                       }

                    reader.Close();

                    return userProfile;
                }
            }
        }






        //GetByIdWithPostsAndComments note that comments userprofile id is not u.Id it could be another user writing commennts on your post

















        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (Name, Email, DateCreated, ImageUrl, Bio)
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Email, @DateCreated, @ImageUrl, @Bio)";

                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", userProfile.Bio);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET Name = @Name,
                               Email = @Email,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               Bio = @Bio
                         WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", userProfile.Id);
                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", userProfile.Bio);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    }
