using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Gifter.Models;
using Gifter.Utils;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Gifter.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(IConfiguration configuration) : base(configuration) { }

        //public List<Post> GetAll()
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                  SELECT Id, Title, Caption, DateCreated, ImageUrl, UserProfileId
        //                    FROM Post
        //                ORDER BY DateCreated";

        //            var reader = cmd.ExecuteReader();

        //            var posts = new List<Post>();
        //            while (reader.Read())
        //            {
        //                posts.Add(new Post()
        //                {
        //                    Id = DbUtils.GetInt(reader, "Id"),
        //                    Title = DbUtils.GetString(reader, "Title"),
        //                    Caption = DbUtils.GetString(reader, "Caption"),
        //                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
        //                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
        //                    UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
        //                });
        //            }

        //            reader.Close();

        //            return posts;
        //        }
        //    }
        //}

        //new GetAll: single round trip to the database.
        public List<Post> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                       p.ImageUrl AS PostImageUrl, p.UserProfileId,

                       up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                       up.ImageUrl AS UserProfileImageUrl
                  FROM Post p 
                       LEFT JOIN UserProfile up ON p.UserProfileId = up.id
              ORDER BY p.DateCreated";

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add(new Post()
                        {
                            Id = DbUtils.GetInt(reader, "PostId"),
                            Title = DbUtils.GetString(reader, "Title"),
                            Caption = DbUtils.GetString(reader, "Caption"),
                            DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            UserProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "UserProfileId"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                            },
                        });
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public List<Post> GetAllWithComments()
        {
            using (var conn = Connection)// Opens a database connection using a predefined Connection object.
            {
                conn.Open(); //Establishes the database connection.
                using (var cmd = conn.CreateCommand()) //Creates a SQL command object to execute a query.
                {
                    //SQL query selects fields from the Post, UserProfile, and Comment tables.
                    //LEFT JOIN: Ensures that all posts are included, even if there are no associated comments or user profiles.
                                        cmd.CommandText = @"
                SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
                       p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                       up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
                       up.ImageUrl AS UserProfileImageUrl,

                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                  FROM Post p
                       LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                       LEFT JOIN Comment c on c.PostId = p.id
              ORDER BY p.DateCreated";

                    var reader = cmd.ExecuteReader(); //Executes the query and returns a data reader to iterate through the results.

                    var posts = new List<Post>(); //Initializes a list to hold the posts.
                    while (reader.Read()) // Iterates over each row of the result set. //reader is instance of SqlDataReader, which is used to read data retrieved from a SQL query. 
                    {
                        //Note:  DbUtils is utility class that provides helper methods for interacting with database data. 
                        var postId = DbUtils.GetInt(reader, "PostId");  //"PostId" is the name of the column bcs alias defined in the SQL query (p.Id AS PostId).

                        var existingPost = posts.FirstOrDefault(p => p.Id == postId); //Checks if the post already exists in the list.
                        if (existingPost == null) //Creates a new Post object if it doesn’t already exist in the list.
                        {
                            existingPost = new Post()
                            {
                                Id = postId,
                                Title = DbUtils.GetString(reader, "Title"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                                UserProfile = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "PostUserProfileId"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                },
                                Comments = new List<Comment>()
                            };

                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            //Adds comments to the post if they exist:
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();

                    return posts; //Returns the list of posts, each with its associated comments and user profile.
                }
            }
        }



        public Post GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId, 
                                 u.Id AS userId, u.Name AS userName, u.Email AS userEmail, u.ImageUrl AS userImageUrl, u.DateCreated AS userDateCreated
                           FROM Post p
                           LEFT JOIN UserProfile u ON u.Id=p.UserProfileId
                           WHERE p.Id = @Id";
                    //added p.Id in SELECT
                    //USE LEFT join
                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    if (reader.Read())
                    {
                        post = new Post()
                        {
                            Id = id,
                            Title = DbUtils.GetString(reader, "Title"),
                            Caption = DbUtils.GetString(reader, "Caption"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            //Update the PostRepository.GetById() method to include the UserProfile object in the returned Post object.
                            UserProfile= new UserProfile
                            {
                                Id = DbUtils.GetInt(reader, "userId"), // Corrected from `id` to `userId`
                                Name = DbUtils.GetString(reader, "userName"),
                                Email = DbUtils.GetString(reader, "userEmail"),
                                DateCreated = DbUtils.GetDateTime(reader, "userDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "userImageUrl"),
                              
                            }
                        };
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        //MY VERSION: has Userprofile u for Post's user and UserProfile cu for comment's user
        //public Post GetPostByIdWithComments(int id)
        //{
        //    using (var conn = Connection) // Establish a connection to the database
        //    {
        //        conn.Open(); // Open the connection
        //        using (var cmd = conn.CreateCommand()) // Create a command object to execute SQL
        //        {
        //            // SQL query to select a post, its associated user profile, and comments
        //            cmd.CommandText = @"
        //        SELECT p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl,                       
        //               p.UserProfileId, 
        //               u.Id AS userId, u.Name AS userName, u.Email AS userEmail,                  
        //               u.ImageUrl AS userImageUrl, u.DateCreated AS userDateCreated, 
        //               c.Id AS commentId, c.Message AS comment, c.UserProfileId AS commenterUserProfileId,
        //               cu.Name AS commenterName
        //        FROM Post p
        //        JOIN UserProfile u ON u.Id = p.UserProfileId
        //        LEFT JOIN Comment c ON c.PostId = p.Id
        //        LEFT JOIN UserProfile cu ON cu.Id = c.UserProfileId
        //        WHERE p.Id = @Id"
        //            ;
        //            //IMPORTANT: commenterUserProfileId can be diff than p.UserProfileId :
        //            //LEFT JOIN UserProfile cu ON cu.Id = c.UserProfileId

        //            // LEFT JOIN ensures that we get the Post even if it has no comments.
        //            //this sql query retrieves details for the Post, the UserProfile, and any Comments along with their respective user profiles.


        //            // Add the post ID as a parameter to the SQL query
        //            DbUtils.AddParameter(cmd, "@Id", id);

        //            // Execute the query and get the results
        //            var reader = cmd.ExecuteReader();

        //            // Post initialization:
        //            // The Post object is initialized only once, when the first row is read from the result.
        //            // The Post object is created only once to prevent it from being overwritten when multiple comments are found, allowing all comments to be added to the same Post.
        //            // Initialize a Post object to null. We will create it once we have a result.
        //            Post post = null;

        //            // Handling Comments:
        //            // The while loop processes all the rows returned by the query, adding any comments to the Post object's Comments list.
        //            // A Comment object is created for each row where a comment is present and is added to the Comments list.
        //            // Loop through the results returned by the query
        //            while (reader.Read())
        //            {
        //                // If post object is null, create the Post object with its UserProfile
        //                if (post == null)
        //                {
        //                    post = new Post()
        //                    {
        //                        Id = id, // Set the Post ID from the query result
        //                        Title = DbUtils.GetString(reader, "Title"), 
        //                        Caption = DbUtils.GetString(reader, "Caption"), // Get the caption (can be null)
        //                        DateCreated = DbUtils.GetDateTime(reader, "DateCreated"), 
        //                        ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
        //                        UserProfileId = DbUtils.GetInt(reader, "UserProfileId"), // Set UserProfileId

        //                        // Create the UserProfile object from the query result
        //                        UserProfile = new UserProfile
        //                        {
        //                            Id = DbUtils.GetInt(reader, "userId"), 
        //                            Name = DbUtils.GetString(reader, "userName"), 
        //                            Email = DbUtils.GetString(reader, "userEmail"), 
        //                            DateCreated = DbUtils.GetDateTime(reader, "userDateCreated"), 
        //                            ImageUrl = DbUtils.GetString(reader, "userImageUrl"), // Get the user's image URL (can be null)
        //                        },
        //                        // Initialize the Comments list, even if there are no comments yet
        //                        Comments = new List<Comment>()
        //                    };
        //                }

        //                // If a COMMENT EXISTS in this row, add it to the post's Comments list
        //                if (!DbUtils.IsDbNull(reader, "commentId"))
        //                {
        //                    // Create a Comment object and populate it with data from the query result
        //                    Comment comment = new Comment
        //                    {
        //                        Id = DbUtils.GetInt(reader, "commentId"), 
        //                        Message = DbUtils.GetString(reader, "comment"), 
        //                        PostId = id, // Set the Post ID for the comment
        //                        UserProfileId = DbUtils.GetInt(reader, "commenterUserProfileId"), // Set the UserProfileId for the commenter

        //                        // Create the UserProfile object for the commenter
        //                        UserProfile = new UserProfile
        //                        {
        //                            Id = DbUtils.GetInt(reader, "commenterUserProfileId"), // Get commenter ID
        //                            Name = DbUtils.GetString(reader, "commenterName") // Get commenter name
        //                        }
        //                    };

        //                    // Add the comment to the post's Comments list
        //                    post.Comments.Add(comment);
        //                }
        //            }

        //            // Close the reader after processing all rows
        //            reader.Close();

        //            // Return the post object, which now includes its comments 
        //            return post;
        //        }
        //    }
        //}

        //TOMMY'S VERSION:
        public Post GetPostByIdWithComments(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
                       p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                       up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
                       up.ImageUrl AS UserProfileImageUrl,

                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                  FROM Post p
                       LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                       LEFT JOIN Comment c on c.PostId = p.id
                    WHERE p.Id = @Id
                    ORDER BY p.DateCreated";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    while (reader.Read())
                    {

                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = DbUtils.GetInt(reader, "PostId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                                UserProfile = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "PostUserProfileId"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                },
                                Comments = new List<Comment>()
                            };

                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            post.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = post.Id,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public void Add(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                INSERT INTO Post (Title, Caption, DateCreated, ImageUrl, UserProfileId)
                OUTPUT INSERTED.ID
                VALUES (@Title, @Caption, @DateCreated, @ImageUrl, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);
                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public void Update(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Post
                           SET Title = @Title,
                               Caption = @Caption,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               UserProfileId = @UserProfileId
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);
                    DbUtils.AddParameter(cmd, "@Id", post.Id);

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
                    cmd.CommandText = "DELETE FROM Post WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //search Post by Title, results  sorted by date in either an ascending or descending direction.
        //The Search() method builds a SQL query that uses the LIKE operator to find records matching the search criterion and uses the sortDesc parameter to determine the ORDER BY direction.

        //NOTE: The cmd.CommandText property is just a string, so we can append to it as we would any other string.


        //Once we have a repository method, we'll create the new Action in the PostController.
        public List<Post> Search(string criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql =
                        @"SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                        p.ImageUrl AS PostImageUrl, p.UserProfileId,

                        up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                        up.ImageUrl AS UserProfileImageUrl
                    FROM Post p 
                        LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                    WHERE p.Title LIKE @Criterion OR p.Caption LIKE @Criterion";

                    if (sortDescending)
                    {
                        sql += " ORDER BY p.DateCreated DESC";
                    }
                    else
                    {
                        sql += " ORDER BY p.DateCreated";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"%{criterion}%");
                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add(new Post()
                        {
                            Id = DbUtils.GetInt(reader, "PostId"),
                            Title = DbUtils.GetString(reader, "Title"),
                            Caption = DbUtils.GetString(reader, "Caption"),
                            DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            UserProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "UserProfileId"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                            },
                        });
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

    }
}



////fetching posts created on or after the provided date.

//public List<Post> Hottest(DateTime since)
//{
//    using (var conn = Connection)
//    {
//        conn.Open();
//        using (var cmd = conn.CreateCommand())
//        {
//            cmd.CommandText = @"
//                SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
//                       p.ImageUrl AS PostImageUrl, p.UserProfileId,
//                       up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
//                       up.ImageUrl AS UserProfileImageUrl
//                FROM Post p 
//                LEFT JOIN UserProfile up ON p.UserProfileId = up.Id
//                WHERE p.DateCreated >= @Since
//                ORDER BY p.DateCreated DESC";

//            DbUtils.AddParameter(cmd, "@Since", since);

//            var reader = cmd.ExecuteReader();

//            var posts = new List<Post>();
//            while (reader.Read())
//            {
//                posts.Add(new Post()
//                {
//                    Id = DbUtils.GetInt(reader, "PostId"),
//                    Title = DbUtils.GetString(reader, "Title"),
//                    Caption = DbUtils.GetString(reader, "Caption"),
//                    DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
//                    ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
//                    UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
//                    UserProfile = new UserProfile()
//                    {
//                        Id = DbUtils.GetInt(reader, "UserProfileId"),
//                        Name = DbUtils.GetString(reader, "Name"),
//                        Email = DbUtils.GetString(reader, "Email"),
//                        DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
//                        ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
//                    },
//                });
//            }
//            reader.Close();
//            return posts;
//        }
//    }
//}
