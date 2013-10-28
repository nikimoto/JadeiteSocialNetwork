using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SocialNetwork.Users
{
    public partial class CommentTemplate : System.Web.UI.UserControl
    {
        [Category("Bahavior")]
        [Description("This is the comment template.")]
        public string CommentTextValue { get; set; }
        public string UserCommented { get; set; }
        public DateTime DateCommented { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.CommentText.Text = this.CommentTextValue;
            this.CommentFooter.InnerText = 
                "Commented by " + this.UserCommented + 
                " on " + this.DateCommented.ToString("dd.MM.yyyy"); 
        }
    }
}