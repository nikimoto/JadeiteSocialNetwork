using System;
using System.Linq.Expressions;

namespace SocialNetwork.Models
{
    public class UserModel : AspNetUser
    {
        public byte[] AvatarImage { get; set; }

        public static Expression<Func<AspNetUser, UserModel>> FromUser
        {
            get
            {
                return x => new UserModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    AvatarImage = x.UserDetail.AvatarImage
                };
            }
        }
    }
}