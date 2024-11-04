////BEFORE CREATING APPLICATION VIEWS:
// import React from "react";
// import "./index.css";
// import PostList from "./components/PostList";
// import PostForm from "./components/PostForm";
// import { BrowserRouter } from 'react-router-dom'

// function App() {
//  return (
// <>
// <BrowserRouter>
//     <PostForm />
//     <PostList />
//   </BrowserRouter>
// </>
// )
// }

// export default App;

////AFTER CREATING APPLICATION VIEWS: to test: localhost:3000 and localhost:3000/posts/add
import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import "./App.css";
import ApplicationViews from "./components/ApplicationViews";
import Header from "./components/Header";
// import { PostProvider } from './contexts/PostContext';
//import PostForm from "./components/PostForm";

function App() {
  return (
    <div className="App">
      <Router>
      {/* <PostProvider> */}
      {/* <PostForm/> */}
        {/* want header / navbar to render on all routes! */}
          <Header /> 
          <ApplicationViews />
          {/* </PostProvider> */}
      </Router>
    </div>
  );
}

export default App;