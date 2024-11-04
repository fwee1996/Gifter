// //State value of the posts array, methods to fetch all posts and add a new post. \
// //Note relative urls we are making requests, don't have anything like https://localhost:5001/api/posts. 
// //This is a benefit of adding the proxy attribute in our package.json file.
import React from "react";

const baseUrl = '/api/post';

// Fetch all posts
export const getAllPosts = () => {
    //return fetch(baseUrl)
    return fetch(`${baseUrl}/GetWithComments`)
        .then((res) => res.json());
};


// export const addPost = async (post) => {
//     try {
//         const response = await fetch('/api/post', {
//             method: 'POST',
//             headers: {
//                 'Content-Type': 'application/json',
//             },
//             body: JSON.stringify(post),
//         });

//         if (!response.ok) {
//             const errorData = await response.json();
//             throw new Error(`Error ${response.status}: ${errorData.title || 'An error occurred'}`);
//         }

//         return await response.json();
//     } catch (error) {
//         console.error('Failed to add post:', error);
//         throw error;
//     }
// };

//export const addPost = async (post) => {
//     try {
//         const response = await fetch('/api/post', {
//             method: 'POST',
//             headers: {
//                 'Content-Type': 'application/json',
//             },
//             body: JSON.stringify(post),
//         });

//         if (!response.ok) {
//             const errorData = await response.json();
//             throw new Error(`Error ${response.status}: ${errorData.title || 'An error occurred'}`);
//         }

//         return await response.json();
//     } catch (error) {
//         console.error('Failed to add post:', error);
//         throw error;
//     }
// };


//Tommy's:
export const addPost = async (singlePost) => {
    return fetch(baseUrl,{
        method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(singlePost),
});
};

// Search posts
export const searchPosts = (query, sortDesc) => {
    return fetch(`${baseUrl}/search?q=${encodeURIComponent(query)}&sortDesc=${sortDesc}`)
        .then((res) => res.json());
};

//function that makes fetch call to get single post details
//NOTE This assumes your API is set up to return a post object which includes an array of comments. If you need to make an additional fetch call to get the comments for a post, update the getPost function as needed.
export const getPost = (id) => {
  return fetch(`/api/post/${id}`).then((res) => res.json());
};

//get specific user's post 
//get posts with UserProfileId
export const getUserPosts = (UserProfileId) => {
    //return fetch(baseUrl)
    return fetch(`${baseUrl}/GetByIdWithPosts/${UserProfileId}`)
        .then((res) => res.json());
};