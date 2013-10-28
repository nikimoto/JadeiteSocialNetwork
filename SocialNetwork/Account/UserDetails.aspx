<%@ Page Title="User Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="SocialNetwork.Account.UserDetails" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="UserDetailsPanel">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <div class="row-fluid">
        <div class="span6">
            <section id="detailsForm">
                <p>You're logged in as <strong><%: User.Identity.GetUserName() %></strong>.</p>
                <fieldset class="form-horizontal">
                    <legend>Details for <asp:Literal runat="server" id="LiteralUsername" Mode="Encode"></asp:Literal></legend>
                    <img src="http://www.almostsavvy.com/wp-content/uploads/2011/04/profile-photo.jpg" id="profilePicture" runat="server"
                            width="110" height="125" />
                    <div class="control-group">
                        
                        <asp:Label runat="server" ID="LabelCity" AssociatedControlID="TextBoxCity"
                            CssClass="control-label">City</asp:Label>
                        <div class="controls">
                            <asp:TextBox runat="server" ID="TextBoxCity" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxCity"
                                CssClass="text-error" ValidationExpression="[\w \d]{3,50}"
                                ErrorMessage="The city name must contain only letters."
                                ValidationGroup="Details" />
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label runat="server" ID="LabelCompany" AssociatedControlID="TextBoxCompany"
                            CssClass="control-label">Company</asp:Label>
                        <div class="controls">
                            <asp:TextBox runat="server" ID="TextBoxCompany" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxCompany"
                                CssClass="text-error" ValidationExpression="[\w \d-]{3,50}"
                                ErrorMessage="The company name must contain only letters."
                                ValidationGroup="Details" />
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label runat="server" ID="LabelEmail" AssociatedControlID="TextBoxEmail"
                            CssClass="control-label">E-mail</asp:Label>
                        <div class="controls">
                            <asp:TextBox runat="server" ID="TextBoxEmail" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxEmail"
                                CssClass="text-error" ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$"
                                ErrorMessage="This is not a valid e-mail address."
                                ValidationGroup="Details" />
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label runat="server" ID="LabelBirthDate" AssociatedControlID="TextBoxBirhtDate"
                            CssClass="control-label">Birth Date (dd.mm.yyyy)</asp:Label>
                        <div class="controls">
                            <asp:TextBox runat="server" ID="TextBoxBirhtDate" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxBirhtDate"
                                CssClass="text-error" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[. /.](0[1-9]|1[012])[. /.](19|20)\d\d$"
                                ErrorMessage="Invalid date format."
                                ValidationGroup="Details" />
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label runat="server" ID="LabelUploadImage"
                            CssClass="control-label">Profile Picture</asp:Label>
                        <div class="controls">
                            <asp:FileUpload ID="FileUploadControl" runat="server" />
                            <asp:Button ID="ButtonUploadImage" runat="server" Text="Upload" CssClass="btn"
                                OnClick="ButtonUploadImage_Click" />
                        </div>
                    </div>
                    <div class="form-actions no-color">
                        <asp:Button runat="server" ID="ButtonSubmit" Text="Submit Changes" OnClick="ButtonSubmit_Click"
                            CssClass="btn" ValidationGroup="ChangePassword" />
                        <asp:Button runat="server" ID="ButtonRedirectChangePass" Text="Change Password?" 
                            OnClick="ButtonRedirectChangePass_Click"
                            CssClass="btn" />                                                
                    </div>
                </fieldset>
            </section>
        </div>
    </div>
    </asp:Panel>
</asp:Content>
