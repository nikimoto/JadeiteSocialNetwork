using SocialNetwork.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Error_Handler_Control;

namespace SocialNetwork
{
    public partial class PublicWelcomeScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Load statistics data
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();

            this.LabelTotalUsers.Text = context.AspNetUsers.Count().ToString();
            this.LabelTotalPosts.Text = context.Posts.Count().ToString();
            this.LabelTotalComments.Text = context.Comments.Count().ToString();

            //Grid filtering for logged users
            if (User.Identity.IsAuthenticated)
            {
                this.ButtonShowAllUsers.Visible = true;
                this.ButtonShowFriends.Visible = true;
                this.SearchPeopleButton.Visible = true;
            }
        }

        //Style changes on mouse over in main Grid
        protected void GridViewUsers_RowDataBound(object sender,
            GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] =
                    "this.style.background='#EEEEEE';this.style.cursor='hand'";
                e.Row.Attributes["onmouseout"] =
                    "this.style.background='white'";
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] =
                    ClientScript.GetPostBackClientHyperlink(
                    this.GridViewUsers, "Select$" + e.Row.RowIndex);
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string username = GridViewUsers.SelectedRow.Cells[1].Text;

            Response.Redirect("/Users/Profile?userID=" + GetUserId(username));
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
        }

        //Extracting userId from username
        public string GetUserId(string username)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();

            var user = context.AspNetUsers.FirstOrDefault(x => x.UserName == username);
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return "";
            }
        }

        //Populating main Grid with all users data
        public IQueryable<object> GetAllUsersGridData(string userID = null)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();

            var result = from users in context.AspNetUsers.Include("Posts").Include("UserDetails")
                         where users.Posts.Count > 0
                         select new
                         {
                             ID = users.Id,
                             Username = users.UserName,
                             LatestPost =
                             users.Posts.OrderByDescending(x => x.DateCreated).FirstOrDefault(),
                             AvatarImage = users.UserDetail.AvatarImage
                         };

            return result;

        }

        //Populating main Grid with friends data
        public IQueryable<object> GetFriends()
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();

            string userId = User.Identity.GetUserId();

            var curUser = context.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();

            var result = from users in context.AspNetUsers.Include("Posts").Include("UserDetail")
                         where
                         users.Friends.Select(x => x.Id).Contains(userId)
                         && users.BefriendedBy.Select(x => x.Id).Contains(userId)
                         && users.Posts.Count > 0
                         select new
                         {
                             ID = users.Id,
                             Username = users.UserName,
                             LatestPost = users.Posts.OrderByDescending(x => x.DateCreated).FirstOrDefault(),
                             AvatarImage = users.UserDetail.AvatarImage
                         };

            return result.OrderBy(x => x.LatestPost.DateCreated);
        }

        //Switching for between grif filtering
        protected void ShowAllUsers_Click(object sender, EventArgs e)
        {
            //!!!! Necessary Hack
            this.GridViewUsers.SelectMethod = "alabala";
            //!!!!
            this.GridViewUsers.SelectMethod = "GetAllUsersGridData";
        }

        protected void ShowFriends_Click(object sender, EventArgs e)
        {
            this.GridViewUsers.SelectMethod = "GetFriends";
        }

        //Search by users by name or city
        public IQueryable<object> SearchPeople(string query)
        {
            SocialNetworkDbEntities context = new SocialNetworkDbEntities();

            var result = from users in context.AspNetUsers.Include("UserDetails")
                         where users.UserName.StartsWith(query) || users.UserDetail.City.StartsWith(query)
                         select new UserLink
                         {
                             ID = users.Id,
                             Username = users.UserName,
                             City = users.UserDetail.City
                         };

            return result;

        }

        //Validating search query
        protected void SearchPeopleButton_Click(object sender, EventArgs e)
        {
            string query = this.QuerySearchPeople.Text;

            if (query.Length <= 2)
            {
                ErrorSuccessNotifier.AddErrorMessage("Query string too short.");
                return;
            }
            else if (query.Length > 200)
            {
                ErrorSuccessNotifier.AddErrorMessage("Query string too long.");
                return;
            }
            else
            {
                BindListView(query);
            }

        }

        //Getting image url from database stream
        protected string GetImageUrl(object imageStream)
        {
            string result;

            if (imageStream != null)
            {
                result = "data:image/jpg;base64," + Convert.ToBase64String((byte[])imageStream);
            }
            else
            {
                result = "~/img/profile-photo.jpg";
            }

            return result;
        }

        protected void ListViewSearchPeople_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            

            //set current page startindex, max rows and rebind to false
            this.DataPagerSearchResults.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            //rebind List View
            BindListView(ViewState["latestquery"].ToString());
        }

        void BindListView(string query)
        {
            this.ListViewSearchPeople.DataSource = SearchPeople(query).ToList();
            this.DataBind();

            ViewState["latestquery"] = query;
        }
    }

    //ItemType used in Listview
    public class UserLink
    {
        public string Username { get; set; }

        public string City { get; set; }

        public string ID { get; set; }
    }
}