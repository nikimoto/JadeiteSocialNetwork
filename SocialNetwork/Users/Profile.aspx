<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SocialNetwork.Users.Profile" %>

<asp:Content ID="ProfileContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="ProfilePanel" CssClass="align-center">
        <asp:Panel runat="server" ID="ProfileHeaderPanel" CssClass="panel">
            <h2 runat="server" id="UsernameLiteral" />
            <asp:Image runat="server" ID="ProfileImage"
                Width="115"
                Height="125"
                AlternateText="Profile Picture" />
            <asp:Panel runat="server" ID="AdminPanel" Visible="False" CssClass="panel">
                <asp:LinkButton runat="server" ID="BannUserLinkButton" Text="Bann User"
                    OnClick="BannUserLinkButton_OnClick" CssClass="link-button"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="RemoveBannLinkButton" Text="Remove Bann"
                    OnClick="RemoveBannLinkButton_OnClick" CssClass="link-button"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="CreateAdminLinkButton" Text="Create Admin"
                    OnClick="CreateAdminLinkButton_OnClick" CssClass="link-button"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="RemoveAdminLinkButton" Text="Remove Admin"
                    OnClick="RemoveAdminLinkButton_OnClick" CssClass="link-button"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="ButtonEditInfo" Text="Edit Info"
                    OnCommand="ButtonEditInfo_Command" CssClass="link-button"></asp:LinkButton>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="AddFriendPanel">
            <asp:LinkButton runat="server" ID="AddFriendLinkButton"
                CommandName="AddFriendCommand"
                OnCommand="AddFriendLinkButton_OnCommand"
                Text="Add Friend"
                CssClass="link-button"></asp:LinkButton>
        </asp:Panel>
        <asp:Panel runat="server" ID="VisibleInfoPanel">
            <asp:Panel ID="profile_about" runat="server" CssClass="panel">
                <h2>About</h2>
                <div class="align-left-margin">
                    <asp:Literal runat="server" ID="EmailLiteral" Text="Email: " />
                    <br />
                    <asp:Literal runat="server" ID="BirthDateLiteral" Text="Birth Date: " />
                    <br />
                    <asp:Literal runat="server" ID="CityLiteral" Text="City: " />
                    <br />
                    <asp:Literal runat="server" ID="CompanyLiteral" Text="Company: " />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="firend_requests_container" CssClass="panel">
                <h2>Friends</h2>
                <asp:Button runat="server" ID="ShowFriendRequestsButton"
                    OnClick="ShowFriendRequestsButton_OnClick"
                    Text="View Friend Requests"
                    Visible="False" CssClass="link-button"
                    AutoPostback="False" />
                <asp:LinkButton runat="server" ID="ViewAllFriendLinkButton"
                    OnClick="ViewAllFriendLinkButton_OnClick"
                    Text="View Friends" CssClass="link-button"></asp:LinkButton>
                <div class="align-left-margin">
                    <asp:ListView runat="server" ID="FriendRequestListView"
                        ItemType="SocialNetwork.Models.AspNetUser"
                        DataKeyNames="Id"
                        Visible="False">
                        <LayoutTemplate>
                            <ul>
                                <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <%#: Item.UserName %>
                                <asp:LinkButton runat="server" ID="ViewRequestProfileLinkButton"
                                    Text="View Profile"
                                    CommandName="ViewRequestProfile"
                                    CommandArgument="<%# Item.Id %>"
                                    OnCommand="ViewRequestProfileLinkButton_OnCommand"
                                    CssClass="link-button"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="AcceptFriendRequestLinkButton"
                                    Text="Accept Request"
                                    CommandName="AcceptFriendRequest"
                                    CommandArgument="<%# Item.Id %>"
                                    OnCommand="AcceptFriendRequestLinkButton_OnCommand"
                                    CssClass="link-button">                            
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="DenyFriendRequestLinkButton"
                                    Text="Deny Request"
                                    CommandName="DenyFriendRequest"
                                    CommandArgument="<%# Item.Id %>"
                                    OnCommand="DenyFriendRequestLinkButton_OnCommand"
                                    CssClass="link-button"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <p>No new friend requests!</p>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="user_post_container" CssClass="panel">
                <h2>Latest posts</h2>
                <div class="align-left-margin">
                    <asp:ListView ID="PostsListView" runat="server"
                        DataKeyNames="PostId"
                        ItemType="SocialNetwork.Models.Post">
                        <LayoutTemplate>
                            <ul>
                                <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                            </ul>
                        </LayoutTemplate>
                        <ItemSeparatorTemplate>
                            <hr />
                        </ItemSeparatorTemplate>
                        <ItemTemplate>
                            <li>
                                <%#: Item.Text.Length > 80 ? Item.Text.Substring(0, 77) + "..." : Item.Text %>
                                <br />
                                Posted on:
                        <asp:Literal runat="server" ID="PostDateCreatedLiteral"
                            Text="<%# Item.DateCreated %>"></asp:Literal>
                                <br />
                                <asp:LinkButton runat="server" ID="PostDetailsLinkButton"
                                    CommandName="ViewPostDetails"
                                    CommandArgument="<%# Item.PostId %>"
                                    OnCommand="PostDetailsLinkButton_OnCommand"
                                    Text="View Details" CssClass="link-button"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <h3>No posts to display!</h3>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:LinkButton runat="server" ID="BackToHomeLinkButton"
            Text="Back to all users"
            Visible="False"
            OnClick="BackToHomeLinkButton_OnClick"
            CssClass="link-button"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
