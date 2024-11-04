using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string ImageUrl { get; set; }
        public string Bio { get; set; } //I ADDED

        [Required]
        public DateTime DateCreated { get; set; }

        //public List<Post> Posts { get; set; } //I ADDED

        // Make Posts property optional
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}