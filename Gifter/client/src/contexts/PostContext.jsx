// import React, { createContext, useContext } from "react";
// import { addPost as addPostAPI } from "../services/PostService"; // Adjust import path as needed

// const PostContext = createContext();

// export const PostProvider = ({ children }) => {
//   const addPost = (post) => {
//     return addPostAPI(post);
//   };

//   return (
//     <PostContext.Provider value={{ addPost }}>
//       {children}
//     </PostContext.Provider>
//   );
// };

// export const usePostContext = () => useContext(PostContext);








// src/contexts/PostContext.jsx

// import React, { createContext, useState, useContext } from 'react';

// // Create a Context for the Posts
// const PostContext = createContext();

// // Create a Provider component
// export const PostProvider = ({ children }) => {
//   const [posts, setPosts] = useState([]);

//   const addPost = async (post) => {
//     try {
//       const payload = {
//         title: post.title,
//         imageUrl: post.imageUrl,
//         caption: post.caption || null,
//         dateCreated: new Date().toISOString(),
//         userProfileId: post.userProfileId,
//         comments: post.comments || null,
//         userProfile: post.userProfile || { id: post.userProfileId }  // Ensure this field is included
//       };

//       console.log('Payload:', payload);  // Log payload for debugging

//       const response = await fetch('/api/post', {
//         method: 'POST',
//         headers: {
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(payload),
//       });

//       if (!response.ok) {
//         const errorData = await response.json();
//         console.error('Server Error:', errorData);  // Log server response error
//         throw new Error(errorData.title || 'Failed to add post');
//       }

//       const newPost = await response.json();
//       setPosts((prevPosts) => [...prevPosts, newPost]);
//     } catch (error) {
//       console.error('Error adding post:', error);
//       throw error;
//     }
//   };

//   return (
//     <PostContext.Provider value={{ posts, addPost }}>
//       {children}
//     </PostContext.Provider>
//   );
// };

// // Custom hook to use the PostContext
// export const usePost = () => {
//   const context = useContext(PostContext);
//   if (context === undefined) {
//     throw new Error('usePost must be used within a PostProvider');
//   }
//   return context;
// };


// // Export the context itself for use in other parts of the app
// export { PostContext };
