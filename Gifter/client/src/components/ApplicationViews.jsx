import { Routes, Route, Navigate} from "react-router-dom";
import PostList from "./PostList";
import PostForm from "./PostForm";
import PostDetails from "./PostDetails";
import UserPosts from "./UserPosts";

const ApplicationViews = () => {

return (
    // <'Routes'> and <Route> components are ones we get from the npm module react-router-dom. The Routes component is going to look at the url and render the first route that is a match.
     <Routes>
     
        {/* <Route> component. If a url matches the value of the path attribute, the children of that <Route> will be what gets rendered. As we've seen before, URLs often have route params in them. */}
        <Route path="/" element= {<PostList />} />
        
        <Route path="/posts/add" element={<PostForm />} />
        
        {/* route here with : id is an example of a path with a route param: /posts/:id. Using the colon, we can tell the react router that this will be some id parameter. These examples of paths that would match this route:
        /posts/5     or          /posts/foo
        steps: import into  App.jsx  and  wrap in <Router> component.
        For test: http://localhost:5173/ and http://localhost:5173/posts/add and http://localhost:5173/posts/1 */}
        <Route path="/posts/:id" element={<PostDetails/>} />

        {/* Note : "*" is catch all for any routes that dont match above defined routes */}
        <Route path="*" element={<p>Whoops, nothing here...</p>} />
     
        {/* Add a new route in ApplicationViews whose path is users/:id and make a new component called UserPosts to go inside that route. If the url is /users/1, the app should only show the posts made by the user with the Id of 1 */}
        <Route path="/users/:id" element={<UserPosts/>} />
     </Routes>
    
    )
  

};

export default ApplicationViews;
