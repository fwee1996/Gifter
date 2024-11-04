// fetch user profile by ID
export const getUserProfileById = async (id) => {
    try {
        const response = await fetch(`/api/UserProfile/${id}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error('Failed to fetch user profile:', error);
        throw error;
    }
};


//Tommy's:
// const baseUrl = 'https://localhost:5001/api/UserProfile';

// export const getUserByIdWithPosts = (id) => {
//     return fetch(`${baseUrl}/GetUserByIdWithPosts/${id}`).then((res) => res.json());
// };