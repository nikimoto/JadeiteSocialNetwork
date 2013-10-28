using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;

namespace SocialNetwork.Users
{
    public partial class ViewFriends : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public IQueryable<UserModel> FriendsGridView_GetData()
        {
            string currentUserId = User.Identity.GetUserId();            
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            var currentUser = context.AspNetUsers.Include("UserDetail").Include("Friends").
                FirstOrDefault(u => u.Id == currentUserId);

            return currentUser.Friends.AsQueryable().Select(UserModel.FromUser);
        }

        protected string GetImage(object img)
        {
            string result = "";
            if (img != null)
            {
                result = "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
            }
            else
            {
                result = "~/img/profile-photo.jpg";
            }

            return result;
        }

        protected void ViewProfileLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            var frienId = e.CommandArgument;
            Response.Redirect("~/Users/Profile.aspx?userID=" + frienId);
        }
    }
}