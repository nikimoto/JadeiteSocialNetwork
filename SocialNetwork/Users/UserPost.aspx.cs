using System;
using System.Linq;
using System.Web.UI.WebControls;
using Error_Handler_Control;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;

namespace SocialNetwork.Users
{
    public partial class UserPost : System.Web.UI.Page
    {
        bool isNewPost = false;
        private string userId;
        private int postId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole("banned"))
            {
                this.UserPostPanel.Visible = false;
                ErrorSuccessNotifier.AddErrorMessage("Banned!");
            }
            else
            {
                this.postId =
                    Convert.ToInt32(Request.Params["postId"]);
                this.isNewPost = (this.postId == 0);

                if (this.isNewPost)
                {
                    this.userId = User.Identity.GetUserId();
                    FillPageContent();
                }
                else
                {
                    SocialNetworkDbEntities context = new SocialNetworkDbEntities();
                    var post = context.Posts.Find(this.postId);
                    if (post != null)
                    {
                        if (post.AspNetUser.Friends.Any(usr => usr.Id == User.Identity.GetUserId()) ||
                            post.AspNetUser.Id == User.Identity.GetUserId())
                        {
                            FillPageContent();
                        }
                        else
                        {
                            ErrorSuccessNotifier.AddErrorMessage("You can view posts only by your friends");
                        }
                    }
                    else
                    {
                        ErrorSuccessNotifier.AddErrorMessage("Post not found");
                    }
                }
            }
        }

        private void FillPageContent()
        {
            if (!isNewPost)
            {
                using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
                {
                    Post post = context.Posts.Include("Comments").Include("AspNetUser").
                        FirstOrDefault(p => p.PostId == this.postId);

                    this.HeaderUsername.InnerText = "Post by " + post.AspNetUser.UserName +
                        " - " + post.Votes.ToString() + " votes";
                    this.TextBoxPostText.Text = post.Text;
                    this.RepeaterComments.DataSource = post.Comments.OrderByDescending(cmnt => cmnt.DateCreated).ToList();
                    this.RepeaterComments.DataBind();

                    this.ViewPostPanel.Visible = true;
                    if (post.UsersLiked.Any(usr => usr.Id == User.Identity.GetUserId()))
                    {
                        LinkButtonVote.Visible = false;
                    }
                }
            }
            else
            {
                this.AddPostPanel.Visible = true;
            }
        }

        protected void Post_Command(object sender, CommandEventArgs e)
        {
            using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
            {
                try
                {
                    Post post = new Post();
                    context.Posts.Add(post);
                    post.AuthorId = User.Identity.GetUserId();
                    post.DateCreated = DateTime.Now;
                    post.Text = Server.HtmlEncode(this.TextBoxNewPostText.Text);
                    context.SaveChanges();
                    Response.Redirect("UserPost.aspx?postId=" + post.PostId, false);
                }
                catch (Exception ex)
                {
                    ErrorSuccessNotifier.AddErrorMessage("Posting failed! Server error!");
                }
            }
        }

        protected void LinkButtonAddComment_Click(object sender, EventArgs e)
        {
            using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
            {
                if (!string.IsNullOrEmpty(this.TextBoxNewComment.Text))
                {
                    try
                    {
                        Comment comment = new Comment();
                        context.Comments.Add(comment);
                        comment.PostId = this.postId;
                        comment.DateCreated = DateTime.Now;
                        comment.UserId = User.Identity.GetUserId();
                        comment.Text = Server.HtmlEncode(this.TextBoxNewComment.Text);
                        context.SaveChanges();
                        Response.Redirect("UserPost.aspx?postId=" + comment.PostId, false);
                    }
                    catch (Exception ex)
                    {
                        ErrorSuccessNotifier.AddErrorMessage("Comment failed! Server error!");
                    }
                }
                else
                {
                    ErrorSuccessNotifier.AddErrorMessage("Cannot post empty comment!");
                }
            }
        }

        protected void LinkButtonVote_Click(object sender, EventArgs e)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();
            using (context)
            {
                var post = context.Posts.Find(this.postId);
                if (post != null && !(post.UsersLiked.Any(usr => usr.Id == User.Identity.GetUserId())))
                {
                    var currentUser = context.AspNetUsers.Find(User.Identity.GetUserId());
                    if (currentUser != null)
                    {
                        try
                        {
                            post.Votes++;
                            post.UsersLiked.Add(currentUser);
                            context.SaveChanges();

                            FillPageContent();
                            this.LinkButtonVote.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            ErrorSuccessNotifier.AddErrorMessage("Voting failed! Server error!");
                        }
                    }
                    else
                    {
                        ErrorSuccessNotifier.AddErrorMessage("User account error! Contact administrator...");
                    }
                }
                else
                {
                    ErrorSuccessNotifier.AddErrorMessage("Error occured while registering your vote!" +
                        Environment.NewLine +
                        "Either post doesn't exist or you have already gave your vote");
                }
            }
        }
    }
}