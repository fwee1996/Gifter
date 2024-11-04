//Instead of making PostList do all the work add this Post component to break up the code
//using the Card component that comes with reactstrap to organize some of the post details.
import React from "react";
import { Card, CardImg, CardBody } from "reactstrap";
import { Link } from "react-router-dom"; //for link to post details


export const Post = ({ post }) => {
  return (
    <Card className="m-4">
      <p className="text-left px-2"> Posted by:
        {/* link to post details */}
        <Link to={`/users/${post.userProfileId}`}>
          <strong>{post.userProfile.name}</strong>
          </Link>
        {/* Posted by: {post.userProfile.name} */}
        </p>
      <CardImg top src={post.imageUrl} alt={post.title} />
      <CardBody>
        <p>
            {/* link to post details */}
          <Link to={`/posts/${post.id}`}>
          <strong>{post.title}</strong>
          </Link>
        </p>
        <p>{post.caption}</p>

        <div className="comments-section">
                    {post.comments && post.comments.length > 0 ? (
                        <div>
                            <h5>Comments:</h5>
                            <ul>
                                {post.comments.map(comment => (
                                    <li key={comment.id}>
                                        <strong>{comment.userProfileId}:</strong> {comment.message}
                                    </li>
                                ))}
                            </ul>
                        </div>
                    ) : (
                        <p>No comments yet.</p>
                    )}
                </div>
      </CardBody>
    </Card>
  );
};

export default Post;