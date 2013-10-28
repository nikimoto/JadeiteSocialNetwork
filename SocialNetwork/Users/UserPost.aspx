<%@ Page Title="Post Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="UserPost.aspx.cs"
    Inherits="SocialNetwork.Users.UserPost" %>

<%@ Register Src="CommentTemplate.ascx" TagName="Comment"
    TagPrefix="templates" %>

<asp:Content ID="ContentPost"
    ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="UserPostPanel">
        <div class="align-center">
            <asp:Panel runat="server" ID="AddPostPanel" Visible="False" CssClass="panel">
                <h2>Add new post</h2>
                <asp:Label ID="AddPostLabel" runat="server"
                    AssociatedControlID="TextBoxNewPostText"
                    Text="Add New Post:">
                    <asp:TextBox ID="TextBoxNewPostText" runat="server"
                        TextMode="MultiLine" Rows="3" Columns="20" />
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Can not add empty post"
                        ControlToValidate="TextBoxNewPostText" Display="Dynamic" />
                </asp:Label>
                <asp:LinkButton ID="LinkButtonNewPost" Text="Post" runat="server"
                    CommandName="Post" CssClass="link-button"
                    OnCommand="Post_Command" />
            </asp:Panel>

            <asp:Panel runat="server" ID="ViewPostPanel" Visible="False" CssClass="panel">
                <asp:Panel runat="server" ID="HeaderPanel">
                    <h2 id="HeaderUsername" runat="server"></h2>
                </asp:Panel>

                <p class="post-text">
                    <asp:Literal ID="TextBoxPostText" runat="server" Mode="Encode" />
                </p>
                <asp:LinkButton ID="LinkButtonVote" runat="server" CssClass="link-button"
                    Text="Vote" OnClick="LinkButtonVote_Click" /><br />
               
                    <asp:Panel runat="server" ID="AddCommentPanel" CssClass="panel">
                        <h2>Comments</h2>
                        <div class="align-left-margin">
                            <ul class="comment-container">
                                <asp:ListView ID="RepeaterComments" runat="server"
                                    ItemType="SocialNetwork.Models.Comment">
                                    <ItemTemplate>
                                        <li>
                                            <templates:Comment ID="CommentBody" runat="server"
                                                CommentTextValue="<%#: Item.Text %>"
                                                UserCommented="<%#: Item.AspNetUser.UserName %>"
                                                DateCommented="<%# Item.DateCreated %>" />
                                        </li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ul>
                        </div>
                        <asp:Label runat="server" ID="AddCommentLabel"
                            AssociatedControlID="TextBoxNewComment"
                            Text="New Comment: ">
                            <asp:TextBox ID="TextBoxNewComment" runat="server"
                                TextMode="MultiLine" Rows="3" Columns="20" />
                        </asp:Label>
                        <asp:LinkButton ID="LinkButtonAddComment" runat="server" CssClass="link-button"
                            Text="Add" OnClick="LinkButtonAddComment_Click" />
                    </asp:Panel>
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
