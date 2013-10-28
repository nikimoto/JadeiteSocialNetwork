using Error_Handler_Control;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialNetwork.Account
{
    public partial class UserDetails : System.Web.UI.Page
    {
        private string userId;

        private bool isInputValidated = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userId = User.Identity.GetUserId();

            if (User.IsInRole("admin"))
            {
                var userIdUrl = Request.Params["userID"];

                if (userIdUrl != null && userIdUrl.Length > 0)
                {
                    this.userId = userIdUrl;
                }
            }
            else if (User.IsInRole("banned"))
            {
                this.UserDetailsPanel.Visible = false;
            }
        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            if (!User.IsInRole("banned"))
            {
                using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
                {
                    var user = context.AspNetUsers.FirstOrDefault(usr => usr.Id == this.userId);
                    if (user != null)
                    {
                        if (this.isInputValidated)
                        {

                            this.LiteralUsername.Text = user.UserName;
                            this.TextBoxCity.Text = user.UserDetail.City;
                            this.TextBoxCompany.Text = user.UserDetail.Company;
                            this.TextBoxEmail.Text = user.UserDetail.Email;
                            var dateAsString = String.Format("{0:dd.MM.yyyy}", user.UserDetail.BirthDate);
                            this.TextBoxBirhtDate.Text = dateAsString;
                            if (user.UserDetail.AvatarImage != null)
                            {
                                this.profilePicture.Src = "data:image/jpeg;base64," +
                                                          Convert.ToBase64String(user.UserDetail.AvatarImage);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("~/");
                    }
                }
            }
            else
            {
                ErrorSuccessNotifier.AddErrorMessage("Banned!");
            }
        }

        protected void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            if (!this.FileUploadControl.HasFile)
            {
                ErrorSuccessNotifier.AddErrorMessage("No image attached.");
                return;
            }
            if(this.FileUploadControl.PostedFile.ContentType == "image/jpeg" ||
                this.FileUploadControl.PostedFile.ContentType == "image/png" ||
                this.FileUploadControl.PostedFile.ContentType == "image/gif")
            {
                int length = FileUploadControl.PostedFile.ContentLength;
                if (length > 819200) // 800Kb = 1024 * 800
                {
                    ErrorSuccessNotifier.AddErrorMessage("The lenght of image is too big. Image must be less than 800kb.");
                    return;
                }

                using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
                {
                    var user = context.AspNetUsers.FirstOrDefault(usr => usr.Id == this.userId);
                    if (user != null)
                    {
                        byte[] fileData = new byte[length + 1];
                        Stream fileStream = FileUploadControl.PostedFile.InputStream;
                        fileStream.Read(fileData, 0, length);

                        user.UserDetail.AvatarImage = fileData;
                        context.SaveChanges();
                        this.profilePicture.Src = "data:image/jpeg;base64," + Convert.ToBase64String(fileData);
                    }
                }
            }
            else
            {
                ErrorSuccessNotifier.AddErrorMessage("The file was not in correct format! Only .jpeg, .png and .gif are supported.");
            }
        }

        private bool CheckIsValid(string validationGroup)
        {
            this.Page.Validate(validationGroup);
            if (this.Page.IsValid)
            {
                this.isInputValidated = true;
                return true;
            }
            else
            {
                this.isInputValidated = false;
                return false;
            }
        }
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (this.CheckIsValid("Details"))
            {
                using (SocialNetworkDbEntities context = new SocialNetworkDbEntities())
                {
                    var user = context.AspNetUsers.Find(userId);
                    var userDetails = user.UserDetail;

                    userDetails.City = TrimWords(TextBoxCity.Text);
                    userDetails.Company = TrimWords(TextBoxCompany.Text);
                    userDetails.Email = TrimWords(TextBoxEmail.Text);
                    if (!string.IsNullOrEmpty(TextBoxBirhtDate.Text))
                    {
                        userDetails.BirthDate = DateTime.Parse(TextBoxBirhtDate.Text);
                    }

                    context.SaveChanges();
                }

                ErrorSuccessNotifier.AddSuccessMessage("Details Succsessfuly changed");
            }
        }

        protected void ButtonRedirectChangePass_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Manage");
        }

        private string TrimWords(string text)
        {
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words);
        }

    }
}