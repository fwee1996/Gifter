//add new post

// import React, { useState, useEffect } from 'react';
// import { addPost } from '../services/PostService';
// export const PostForm =()=>{
// const [newPost,setNewPost]=useState({});

// const controllerInputChange=(e)=>{
//   //make a copy of state
//   const newstate={...newPost}
//   newstate[e.target.value]=e.target.value

//   //set newstate
//   setNewPost(newstate)
// }

// //error: need comments and user profile
// //soln: go to Post model in VS
// //use  "?" bcs these are not necessary, they are nested objects that is given by our database
// //public UserProfile? UserProfile { get; set; }
// // Make Comments property optional
// // public List<Comment>? Comments { get; set; } = new List<Comment>();

// //C# error: post.Id(int)>"time overflow" bcs we didnt give time so use DateTime.NOw in Post controler! 
// //Now when run its working with comments : null and userProfile: null
// //props with values in network tab: caption, dateCReated, id, imageUrl, title, userProfileId
// //no form group so no need prevent default
//   return (<div>
//     <input type ="text" placeholder="title" name="title" onChange={(e)=>controllerInputChange(e)}/>
//     <input type ="text" placeholder="imageUrl" name="imageUrl" onChange={(e)=>controllerInputChange(e)}/>
//     <input type ="text"placeholder="caption" name="caption" onChange={(e)=>controllerInputChange(e)}/>
//     <button onClick={()=>addPost(newPost)}>Add Post</button>
//   </div>)
// }


// export default PostForm;



import React, { useState } from 'react';
import { addPost } from '../services/PostService';

////Tommy's version:
//export const PostForm = () => {
//const [newPost, setNewPost] = useState({});
// const controllerInputChange=(e)=>{
//   //make a copy of state
//   const newstate={...newPost}
//   newstate[e.target.value]=e.target.value

//   //set newstate
//   setNewPost(newstate)
// }
export const PostForm = () => {
  // Initialize state with default values
  const [newPost, setNewPost] = useState({
    title: '',
    imageUrl: '',
    caption: '',
  });


// Controller function to handle input changes
  const controllerInputChange = (e) => {
    //1.  make a copy of state
    const { name, value } = e.target; //Updated controllerInputChange to use name attributes of the inputs to correctly update the state.
    //2. set new state
    setNewPost(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

// //error: need comments and user profile
// //soln: go to Post model in VS
// //use  "?" bcs these are not necessary, they are nested objects that is given by our database
// //public UserProfile? UserProfile { get; set; }
// // Make Comments property optional
// // public List<Comment>? Comments { get; set; } = new List<Comment>();

// //C# error: post.Id(int)>"time overflow" bcs we didnt give time so use DateTime.NOw in Post controler! 
// //Now when run its working with comments : null and userProfile: null
// //props with values in network tab: caption, dateCReated, id, imageUrl, title, userProfileId
// //no form group so no need prevent default
////Tommy's:
//   return (<div>
//     <input type ="text" placeholder="title" name="title" onChange={(e)=>controllerInputChange(e)}/>
//     <input type ="text" placeholder="imageUrl" name="imageUrl" onChange={(e)=>controllerInputChange(e)}/>
//     <input type ="text"placeholder="caption" name="caption" onChange={(e)=>controllerInputChange(e)}/>
//     <button onClick={()=>addPost(newPost)}>Add Post</button>
//   </div>)
  return (
    <div>
      <input type="text" placeholder="Title" name="title" value={newPost.title} onChange={controllerInputChange} />
      <input type="text" placeholder="Image URL"name="imageUrl"value={newPost.imageUrl} onChange={controllerInputChange} />
      <input type="text" placeholder="Caption" name="caption" value={newPost.caption} onChange={controllerInputChange} />
      <button onClick={()=>addPost(newPost)}>Add Post</button>
    </div>
  );
};

export default PostForm;



