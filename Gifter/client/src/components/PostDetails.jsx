import React, { useEffect, useState } from "react";
import { ListGroup, ListGroupItem } from "reactstrap";
import { getPost } from "../services/PostService";
import { useParams } from "react-router-dom";
import Post from "./Post";

export const PostDetails = () => {
  const [post, setPost] = useState();
  const { id } = useParams();

  useEffect(() => {
    getPost(id).then(setPost);
  }, []);

  if (!post) {
    return null;
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-sm-12 col-lg-6">
          <Post post={post} />
          <ListGroup>
            {post.comments.map((c) => (
              <ListGroupItem>{c.message}</ListGroupItem>
            ))}
          </ListGroup>
        </div>
      </div>
    </div>
  );
};

export default PostDetails;









// import React, { useState, useEffect } from 'react';

// const PostDetails = () => {
//   const [posts, setPosts] = useState([]); // Initialize with an empty array

//   useEffect(() => {
//     const fetchPosts = async () => {
//       try {
//         const response = await fetch('/api/posts');
//         const data = await response.json();
//         setPosts(data);
//       } catch (error) {
//         console.error('Error fetching posts:', error);
//       }
//     };

//     fetchPosts();
//   }, []); // Empty dependency array ensures this runs once on mount

//   if (!posts || posts.length === 0) {
//     return <div>No posts available</div>; // Handle empty state or loading state
//   }

//   return (
//     <div>
//       {posts.map(post => (
//         <div key={post.id}>
//           <h2>{post.title}</h2>
//           <img src={post.imageUrl} alt={post.title} />
//           <p>{post.caption}</p>
//         </div>
//       ))}
//     </div>
//   );
// };

// export default PostDetails;
