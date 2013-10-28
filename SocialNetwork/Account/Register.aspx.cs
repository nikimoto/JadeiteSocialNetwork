using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using SocialNetwork.Models;

namespace SocialNetwork.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            string userName = UserName.Text;
            var manager = new AuthenticationIdentityManager(new IdentityStore());
            User u = new User(userName) { UserName = userName };
            IdentityResult result = manager.Users.CreateLocalUser(u, Password.Text);
            if (result.Success) 
            {
                manager.Authentication.SignIn(Context.GetOwinContext().Authentication, u.Id, isPersistent: false);
                //OpenAuthProviders.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
                {
                    UserDetail details = new UserDetail();
                    context.UserDetails.Add(details);                    
                    context.SaveChanges();
                    var user = context.AspNetUsers.Find(u.Id);
                    user.UserDetailsId = details.UserDetailsId;
                    context.SaveChanges();
                }

                Response.Redirect("~/Account/UserDetails");
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}