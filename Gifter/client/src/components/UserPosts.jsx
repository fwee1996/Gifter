// //https://localhost:5001/api/userProfile/1
//Add a new route in ApplicationViews whose path is users/:id and make a new component called UserPosts to go inside that route. If the url is /users/1, the app should only show the posts made by the user with the Id of 1
//Update the Post component so the username at the top is a link to your new route

import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const UserPosts = () => {
  const { id } = useParams(); // Retrieve the user ID from the URL
  const [userPosts, setUserPosts] = useState([]);

  useEffect(() => {
    // Fetch posts for the user with the given ID
    fetch(`https://localhost:5001/api/UserProfile/${id}`)
      .then(response => response.json())
      .then(data => {
        setUserPosts(data.posts); // Assume posts are under `posts` in the response
      })
      .catch(error => console.error('Error fetching user posts:', error));
  }, [id]); // Depend on `id` so useEffect runs when `id` changes

  return (
    <div>
      <h1>Posts by User {id}</h1>
      <ul>
        {userPosts.length > 0 ? (
          userPosts.map(post => (
            <li key={post.id}>
              <h2>{post.title}</h2>
              <p>{post.caption}</p>
              <img src={post.imageUrl} alt={post.title} />
              <p>{new Date(post.dateCreated).toLocaleDateString()}</p>
            </li>
          ))
        ) : (
          <p>No posts available for this user.</p>
        )}
      </ul>
    </div>
  );
};

export default UserPosts;
