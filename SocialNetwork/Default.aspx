<%@ Page Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="Default.aspx.cs"
    Inherits="SocialNetwork.PublicWelcomeScreen"
    EnableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome to Jadeite Social Network</h1>

    <asp:Panel runat="server" ID="PanelTime" CssClass="panel align-center">
        <h2>The social network that has the force with it.</h2>
        <asp:UpdatePanel runat='server' ID='UpdatePanelTime' UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="TimerTimeRefresh" EventName="Tick" />
            </Triggers>
            <ContentTemplate>
                <h5>Total Users:
                    <asp:Label ID="LabelTotalUsers" runat="server"></asp:Label></h5>
                <h5>Total Posts:
                    <asp:Label ID="LabelTotalPosts" runat="server"></asp:Label></h5>
                <h5>Total Comments:
                    <asp:Label ID="LabelTotalComments" runat="server"></asp:Label></h5>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel runat="server" ID="PanelGridView" CssClass="panel align-center">
        <asp:UpdatePanel runat='server' ID='UpdatePanelGridView' UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="TimerTimeRefresh" runat="Server" Interval="30000" />
                <h2>Recent Activity:</h2>
                <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False"
                    AllowSorting="true"
                    AllowPaging="True" DataKeyNames="ID" PageSize="3"
                    OnRowDataBound="GridViewUsers_RowDataBound"
                    OnSelectedIndexChanged="OnSelectedIndexChanged"
                    SelectMethod="GetAllUsersGridData"
                    CssClass="table table-striped table-bordered table-condensed">
                    <Columns>
                        <asp:TemplateField HeaderText="Avatar">
                            <ItemTemplate>
                                <asp:Image ImageUrl='<%# GetImageUrl(Eval("AvatarImage")) %>' runat="server"
                                    Width="40px" Height="40px" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Username" HeaderText="User" HtmlEncode="true" SortExpression="Username" />
                        <asp:BoundField DataField="LatestPost.Text" HeaderText="Latest Post" HtmlEncode="true" />
                        <asp:BoundField DataField="LatestPost.DateCreated" HeaderText="Date"
                            DataFormatString="{0:MM/dd/yy HH:mm}" HtmlEncode="true" />
                        <asp:BoundField DataField="LatestPost.Votes" HeaderText="Votes" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="ButtonShowFriends" runat="server"
                    Text="Show Friends activities" OnClick="ShowFriends_Click"
                    Visible="false" CssClass="link-button" />
                <asp:Button ID="ButtonShowAllUsers" runat="server"
                    Text="Show users activities" OnClick="ShowAllUsers_Click"
                    Visible="false" CssClass="link-button"/>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel runat="server" ID="PanelSearchArea" CssClass="panel align-center">
        <asp:UpdatePanel runat='server' ID='UpdatePanelSearchArea' UpdateMode="Conditional">
            <ContentTemplate>
                <h2>Search for friends</h2>
                <asp:Label runat="server" Text="Search social network: " AssociatedControlID="QuerySearchPeople">
                    <asp:TextBox ID="QuerySearchPeople" runat="server"></asp:TextBox>
                </asp:Label>
                <asp:Button ID="SearchPeopleButton" runat="server" 
                    OnClick="SearchPeopleButton_Click" Text="Search" 
                    CssClass="link-button"/>

                <div class="align-left-margin">
                    <asp:ListView ID="ListViewSearchPeople" runat="server" AutoGenerateColumns="False"
                        ItemType="SocialNetwork.UserLink"
                        OnPagePropertiesChanging="ListViewSearchPeople_PagePropertiesChanging"
                        >
                        <LayoutTemplate>
                            <h2>Search Results</h2>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </LayoutTemplate>

                        <ItemSeparatorTemplate>
                            <hr />
                        </ItemSeparatorTemplate>

                        <ItemTemplate>
                            <a href="/Users/Profile.aspx?userID=<%# Item.ID %>"><%#: Item.Username %></a>
                            <span runat="server"
                                visible='<%# string.IsNullOrEmpty(Item.City) ? false : true %>'>
                                from <%#: Item.City %>
                            </span>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <asp:DataPager ID="DataPagerSearchResults" runat="server"
                    PagedControlID="ListViewSearchPeople" PageSize="3"
                    QueryStringField="">
                    <Fields>
                        <asp:NextPreviousPagerField ShowFirstPageButton="true"
                            ShowNextPageButton="false" ShowPreviousPageButton="false" ButtonCssClass="link-button" />
                        <asp:NumericPagerField CurrentPageLabelCssClass="link-button"/>
                        <asp:NextPreviousPagerField ShowLastPageButton="true"
                            ShowNextPageButton="false" ShowPreviousPageButton="false" ButtonCssClass="link-button"/>
                    </Fields>
                </asp:DataPager>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
