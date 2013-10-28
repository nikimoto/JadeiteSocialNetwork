<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewFriends.aspx.cs" Inherits="SocialNetwork.Users.ViewFriends" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Friends</h1>
    <asp:Panel runat="server" CssClass="panel align-center">
        <h2>List of your friends</h2>
        <asp:GridView runat="server" ID="FriendsGridView"
            ItemType="SocialNetwork.Models.UserModel"
            DataKeyNames="Id"
            AllowPaging="True"
            AllowSorting="True"
            PageSize="5"
            SelectMethod="FriendsGridView_GetData"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-condensed">
            <Columns>
                <asp:TemplateField HeaderText="Avatar">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="AcatarImage"
                            ImageUrl="<%# GetImage(Item.AvatarImage) %>"
                            Width="40px"
                            Height="40px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User" SortExpression="UserName">
                    <ItemTemplate>
                        <%#: Item.UserName %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="ViewProfileLinkButton"
                            Text="View Profile"
                            CommandArgument="<%# Item.Id %>"
                            CommandName="ViewProfileCommand"
                            OnCommand="ViewProfileLinkButton_OnCommand"
                            CssClass="link-button" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
