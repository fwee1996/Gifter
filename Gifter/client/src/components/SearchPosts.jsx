import React, { useState } from "react";
import { searchPosts } from "../services/PostService";

export const SearchPosts = ({ onSearch }) => {
    const [query, setQuery] = useState('');
    const [sortDesc, setSortDesc] = useState(false);

    const handleSearch = async (event) => {
        event.preventDefault();
        try {
            const results = await searchPosts(query, sortDesc);
            onSearch(results);
        } catch (error) {
            console.error('Error searching posts:', error);
            alert('Failed to search posts. Please try again.');
        }
    };

    return (
        <form className="search-form" onSubmit={handleSearch}>
            <div className="form-group">
                <label>Search</label>
                <input
                    type="text"
                    className="form-control"
                    placeholder="Search posts"
                    value={query}
                    onChange={(event) => setQuery(event.target.value)}
                />
            </div>
            <div className="form-group">
                <label>Sort Descending</label>
                <input
                    type="checkbox"
                    checked={sortDesc}
                    onChange={() => setSortDesc(!sortDesc)}
                />
            </div>
            <button type="submit" className="btn btn-primary">Search</button>
        </form>
    );
};

export default SearchPosts;
