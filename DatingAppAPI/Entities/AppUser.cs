using System;
using System.Collections.Generic;
using DatingAppAPI.Extensions;
namespace DatingAppAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName {get; set;}
        public byte[] PasswordHash {get; set;}
        public byte[] PasswordSalt {get;set;}
        public DateTime DateOfBirth {get; set;}
        public string KnownAs {get; set;}
        public DateTime Created {get; set;} = DateTime.Now;
        public DateTime LastActive {get; set;} = DateTime.Now;
        public string Gender {get; set;}
        public string Introduction {get; set;}
        public string LookingFor {get; set;}
        public string Interests {get; set;}
        public string City {get; set;}
        public string Country {get; set;}
        public ICollection<Photo> Photos {get; set;}
        // app user can have liked users to whom he has liked.
        public ICollection<UserLike> LikedUsers { get; set; }
        //app user can be liked by other users.
        public ICollection<UserLike> LikedByUsers { get; set; }

        

    }
}