using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Error_Handler_Control;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;

namespace SocialNetwork.Users
{
    public partial class Profile : System.Web.UI.Page
    {
        private string userSavedId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole("admin"))
            {
                this.userSavedId = Request.Params["userID"];
                this.AdminPanel.Visible = true;
            }                       
            else if (User.IsInRole("banned"))
            {
                this.ProfilePanel.Visible = false;
        }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!User.IsInRole("banned"))
            {
            if (!Page.IsPostBack)
            {
                    if (string.IsNullOrEmpty(Request.Params["userID"]) ||
                        Request.Params["userID"] == User.Identity.GetUserId())
                {
                    FillCurrentProfileData();
                }
                else
                {
                    var userId = Request.Params["userID"];
                    FillForeignProfile(userId);
                }
            }

            this.FriendRequestListView.DataBind();
        }
            else
            {
                ErrorSuccessNotifier.AddErrorMessage("Banned!");
            }
        }

        private void FillCurrentProfileData()
        {
            var context = new SocialNetworkDbEntities();
            using (context)
            {
                string currentUserId = User.Identity.GetUserId();
                var user = context.AspNetUsers.Include("Posts").FirstOrDefault(usr => usr.Id == currentUserId);

                if (user == null)
                {
                    Response.Redirect("~");
                }

                this.ProfileImage.ImageUrl = GetImage(user);
                this.UsernameLiteral.InnerText = Server.HtmlEncode(user.UserName);
                this.ShowFriendRequestsButton.Visible = true;
                this.FriendRequestListView.Visible = false;
                this.EmailLiteral.Text += Server.HtmlEncode(user.UserDetail.Email);
                this.BirthDateLiteral.Text += string.Format("{0:dd-MMMM-yyyy}", user.UserDetail.BirthDate);
                this.CityLiteral.Text += Server.HtmlEncode(user.UserDetail.City);
                this.CompanyLiteral.Text += Server.HtmlEncode(user.UserDetail.Company);
                this.PostsListView.DataSource = user.Posts.OrderByDescending(post => post.DateCreated).Take(5);
                this.PostsListView.DataBind();
                this.AddFriendLinkButton.Visible = false;
            }
        }

        private void FillForeignProfile(string userId)
        {
            var context = new SocialNetworkDbEntities();
            using (context)
            {
                try
                {
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = context.AspNetUsers.FirstOrDefault(u => u.Id == currentUserId);

                    var user = context.AspNetUsers.Include("Posts").FirstOrDefault(usr => usr.Id == userId);
                    if (user != null)
                    {
                        this.ProfileImage.ImageUrl = GetImage(user);
                        this.UsernameLiteral.InnerText = Server.HtmlEncode(user.UserName);

                        if (user.Friends.Any(u => u.Id == currentUserId))
                        {
                            this.EmailLiteral.Text += Server.HtmlEncode(user.UserDetail.Email);
                            this.BirthDateLiteral.Text += string.Format("{0:dd-MMMM-yyyy}", user.UserDetail.BirthDate);
                            this.CityLiteral.Text += Server.HtmlEncode(user.UserDetail.City);
                            this.CompanyLiteral.Text += Server.HtmlEncode(user.UserDetail.Company);
                            this.PostsListView.DataSource = user.Posts.OrderByDescending(post => post.DateCreated).Take(5) ;
                            this.PostsListView.DataBind();
                            this.AddFriendLinkButton.Visible = false;
                            this.BackToHomeLinkButton.Visible = false;
                        }
                        else
                        {
                            bool isFriend = CheckIfFriend(userId);
                            if (isFriend)
                            {
                                this.AddFriendLinkButton.Visible = false;
                            }
                            else
                            {
                                this.AddFriendLinkButton.Visible = true;
                            }

                            this.VisibleInfoPanel.Visible = false;
                            this.BackToHomeLinkButton.Visible = true;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("User does not exist!");
                    }
                }
                catch (Exception e)
                {
                    this.AddFriendPanel.Visible = false;
                    this.AdminPanel.Visible = false;
                    this.VisibleInfoPanel.Visible = false;
                    this.ProfileHeaderPanel.Visible = false;
                    this.BackToHomeLinkButton.Visible = true;
                    ErrorSuccessNotifier.AddErrorMessage(e);
                }
            }
        }

        private bool CheckIfFriend(string userId)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            using (context)
            {
                var currentUserId = User.Identity.GetUserId();
                var currentUser = context.AspNetUsers.FirstOrDefault(u => u.Id == currentUserId);

                if (currentUser == null)
                {
                    return false;
                }

                return currentUser.Friends.Any(f => f.Id == userId);
            }
        }

        protected string GetImage(AspNetUser user)
        {
            if (user.UserDetail == null)
            {
                return "~/img/profile-photo.jpg";
            }

            object img = user.UserDetail.AvatarImage;
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

        protected void PostDetailsLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            int postID = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("~/Users/UserPost.aspx?postId=" + postID, false);
        }

        protected void AddFriendLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            try
            {
                string userId = Request.Params["userID"];
                SocialNetworkDbEntities context = new SocialNetworkDbEntities();
                using (context)
                {
                    var user = context.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = context.AspNetUsers.FirstOrDefault(u => u.Id == currentUserId);
                    currentUser.Friends.Add(user);
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage("Friend request sent!");
                    this.AddFriendLinkButton.Visible = false;
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception);
            }
        }

        protected void ViewRequestProfileLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            string userId = e.CommandArgument.ToString();
            Response.Redirect("~/Users/Profile.aspx?userID=" + userId);
        }

        protected void AcceptFriendRequestLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            try
            {
                string friendId = e.CommandArgument.ToString();
                SocialNetworkDbEntities contex = new SocialNetworkDbEntities();
                using (contex)
                {
                    var friend = contex.AspNetUsers.FirstOrDefault(u => u.Id == friendId);
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = contex.AspNetUsers.FirstOrDefault(u => u.Id == currentUserId);
                    currentUser.Friends.Add(friend);
                    contex.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage("Friend request accepted!");
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception);
            }
        }

        protected void ShowFriendRequestsButton_OnClick(object sender, EventArgs e)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            using (context)
            {
                if (!this.FriendRequestListView.Visible)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = context.AspNetUsers.Include("Friends").Include("BefriendedBy").FirstOrDefault(u => u.Id == currentUserId);
                    var befriendedByList = currentUser.BefriendedBy;
                    List<AspNetUser> friendCandidates = new List<AspNetUser>();
                    foreach (var candidate in befriendedByList)
                    {
                        if (!currentUser.Friends.Contains(candidate))
                        {
                            friendCandidates.Add(candidate);
                        }
                    }

                    this.FriendRequestListView.DataSource = friendCandidates;
                    this.FriendRequestListView.DataBind();
                    this.FriendRequestListView.Visible = true;
                }
                else
                {
                    this.FriendRequestListView.Visible = false;
                }
            }
        }

        protected void DenyFriendRequestLinkButton_OnCommand(object sender, CommandEventArgs e)
        {
            try
            {
                string friendId = e.CommandArgument.ToString();
                SocialNetworkDbEntities contex = new SocialNetworkDbEntities();
                using (contex)
                {
                    var friend = contex.AspNetUsers.FirstOrDefault(u => u.Id == friendId);
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = contex.AspNetUsers.FirstOrDefault(u => u.Id == currentUserId);
                    currentUser.BefriendedBy.Remove(friend);
                    friend.Friends.Remove(currentUser);
                    contex.SaveChanges();
                    ErrorSuccessNotifier.AddInfoMessage("Friend request denied!");
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception);
            }
        }

        protected void ButtonEditInfo_Command(object sender, CommandEventArgs e)
        {
            if (User.IsInRole("admin"))
            {
                Response.Redirect("~/Account/UserDetails?userId=" + userSavedId);
            }
            else
            {
                Response.Redirect("~/Account/UserDetails.aspx");
            }

            
        }

        protected void BackToHomeLinkButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void BannUserLinkButton_OnClick(object sender, EventArgs e)
        {
            string userToBannId = Request.Params["userID"];
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            try
            {
                using (context)
                {
                    var userToBann = context.AspNetUsers.FirstOrDefault(u => u.Id == userToBannId);
                    if (userToBann == null)
                    {
                        throw new ArgumentException("User does not exist!");
                    }

                    var role = context.AspNetRoles.FirstOrDefault(r => r.Name == "banned");
                    userToBann.AspNetRoles.Clear();
                    userToBann.AspNetRoles.Add(role);
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage(string.Format("User \"{0}\" banned!", userToBann.UserName));
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception.Message);
            }
        }

        protected void RemoveBannLinkButton_OnClick(object sender, EventArgs e)
        {
            string userToUnbannId = Request.Params["userID"];
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            try
            {
                using (context)
                {
                    var userToUnbann = context.AspNetUsers.FirstOrDefault(u => u.Id == userToUnbannId);
                    if (userToUnbann == null)
                    {
                        throw new ArgumentException("User does not exist!");
                    }

                    userToUnbann.AspNetRoles.Clear();
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage(string.Format("User \"{0}\" bann removed!", userToUnbann.UserName));
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception.Message);
            }
        }

        protected void CreateAdminLinkButton_OnClick(object sender, EventArgs e)
        {
            string newAdminId = Request.Params["userID"];
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            try
            {
                using (context)
                {
                    var newAdmin = context.AspNetUsers.FirstOrDefault(u => u.Id == newAdminId);
                    if (newAdmin == null)
                    {
                        throw new ArgumentException("User does not exist!");
                    }

                    var role = context.AspNetRoles.FirstOrDefault(r => r.Name == "admin");
                    newAdmin.AspNetRoles.Clear();
                    newAdmin.AspNetRoles.Add(role);
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage(string.Format("User \"{0}\" is now admin!", newAdmin.UserName));
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception.Message);
            }
        }

        protected void RemoveAdminLinkButton_OnClick(object sender, EventArgs e)
        {
            string userId = Request.Params["userID"];
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            try
            {
                using (context)
                {
                    var user = context.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                    {
                        throw new ArgumentException("User does not exist!");
                    }

                    user.AspNetRoles.Clear();
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage(string.Format("User \"{0}\" admin rights removed!", user.UserName));
                }
            }
            catch (Exception exception)
            {
                ErrorSuccessNotifier.AddErrorMessage(exception.Message);
            }
        }

        protected void ViewAllFriendLinkButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Users/ViewFriends.aspx");
        }
    }
}