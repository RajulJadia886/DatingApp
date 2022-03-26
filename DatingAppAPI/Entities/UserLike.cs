using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Entities
{
    public class UserLike
    {
        //current source user - who liked other users.
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
         //liked user - other users who liked the current user.
        public AppUser LikedUser { get; set; }
        public int LikedUserId { get; set; }
    }
}