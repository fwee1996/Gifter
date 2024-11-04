// // // //component loads, it calls the getAllPosts method it recieves from the provider 
// // // //and render list of posts.
// // // import React, { useState, useEffect } from "react";
// // // import { getAllPosts } from "../services/PostService";

// // import React, { useState, useEffect } from "react";
// // import { getAllPosts } from "../services/PostService";

// // const PostList = () => {
// //   const [posts, setPosts] = useState([]);

// //   const getPosts = () => {
// //     getAllPosts().then(allPosts => setPosts(allPosts)); 
// //   };

// //   useEffect(() => {
// //     getPosts();
// //   }, []); 



// //   return (  
// //     <div>
// //       {posts.map((post) => (
// //         <div key={post.id}>
// //           <img src={post.imageUrl} alt={post.title} />
// //           <p>
// //             <strong>{post.title}</strong>
// //           </p>
// //           <p>{post.caption}</p>
// //         </div>
// //       ))}
// //     </div>
// //   );
// // };

// // export default PostList;


// //Updated Post list to use Post.jsx component for "Modularization" because the project will get complicated.
// import React, { useState, useEffect } from "react";
// import { getAllPosts } from "../services/PostService";
// import { Post } from "./Post";

// const PostList = () => {
//   const [posts, setPosts] = useState([]);

//   const getPosts = () => {
//     getAllPosts().then(allPosts => setPosts(allPosts)); 
//   };

//   useEffect(() => {
//     getPosts();
//   }, []); 
//   return (
//     <div className="container">
//       <div className="row justify-content-center">
//         <div className="cards-column">
//           {posts.map((post) => (
//             <Post key={post.id} post={post} />
//           ))}
//         </div>
//       </div>
//     </div>
//   );
// };

// export default PostList;























import React, { useState, useEffect } from "react";
import { getAllPosts } from "../services/PostService";
import { Post } from "./Post";
import SearchPosts from "./SearchPosts";

const PostList = () => {
    const [posts, setPosts] = useState([]);

    const fetchPosts = async () => {
            const allPosts = await getAllPosts();
            setPosts(allPosts);
    };

    useEffect(() => {
        fetchPosts();
    }, []);

    const handleSearch = (searchResults) => {
        setPosts(searchResults);
    };

    return (
        <div>
            <SearchPosts onSearch={handleSearch} />
            <div className="post-list">
                {posts.map(post => (
                    <Post key={post.id} post={post} />
                ))}
            </div>
        </div>
    );
};

export default PostList;
