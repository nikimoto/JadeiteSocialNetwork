//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.Comments = new HashSet<Comment>();
            this.Posts = new HashSet<Post>();
            this.AspNetRoles = new HashSet<AspNetRole>();
            this.BefriendedBy = new HashSet<AspNetUser>();
            this.Friends = new HashSet<AspNetUser>();
            this.LikedPosts = new HashSet<Post>();
        }
    
        public string Id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserDetailsId { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual AspNetUserManagement AspNetUserManagement { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        public virtual ICollection<AspNetUser> BefriendedBy { get; set; }
        public virtual ICollection<AspNetUser> Friends { get; set; }
        public virtual ICollection<Post> LikedPosts { get; set; }
    }
}
