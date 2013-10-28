<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="SocialNetwork.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="text-error" />
    </div>

    <div class="row-fluid">
        <div class="span6">
            <%--<section id="detailsForm">--%>
                <p>You're logged in as <strong><%: User.Identity.GetUserName() %></strong>.</p>
                <%--<fieldset class="form-horizontal">
                    <legend>Details Form</legend>
                    <div class="control-group">
                        <asp:Label runat="server" ID="LabelCity" AssociatedControlID="TextBoxCity"
                            CssClass="control-label">City</asp:Label>
                        <div class="controls">
                            <asp:TextBox runat="server" ID="TextBoxCity" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxCity"
                                CssClass="text-error" ValidationExpression="^\p{L}+$"
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
                                CssClass="text-error" ValidationExpression="^\p{L}+$"
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
                        <asp:Button runat="server" ID="ButtonSubmit" Text="Submit" OnClick="ButtonSubmit_Click"
                            CssClass="btn" ValidationGroup="ChangePassword" />                       
                    </div>
                </fieldset>
            </section>--%>
            <section id="passwordForm">
                <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">
                    <p>
                        You do not have a local password for this site. Add a local
                        password so you can log in without an external login.
                    </p>
                    <fieldset class="form-horizontal">
                        <legend>Set Password Form</legend>
                        <div class="control-group">
                            <asp:Label runat="server" AssociatedControlID="password" CssClass="control-label">Password</asp:Label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="password" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="password"
                                    CssClass="text-error" ErrorMessage="The password field is required."
                                    Display="Dynamic" ValidationGroup="SetPassword" />
                                <asp:ModelErrorMessage runat="server" ModelStateKey="NewPassword" AssociatedControlID="password"
                                    CssClass="text-error" SetFocusOnError="true" />
                            </div>
                        </div>

                        <div class="control-group">
                            <asp:Label runat="server" AssociatedControlID="confirmPassword" CssClass="control-label">Confirm password</asp:Label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword"
                                    CssClass="text-error" Display="Dynamic" ErrorMessage="The confirm password field is required."
                                    ValidationGroup="SetPassword" />
                                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                                    CssClass="text-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match."
                                    ValidationGroup="SetPassword" />

                            </div>
                        </div>

                        <div class="form-actions no-color">
                            <asp:Button runat="server" Text="Set Password" ValidationGroup="SetPassword" OnClick="SetPassword_Click" CssClass="btn" />
                        </div>
                    </fieldset>
                </asp:PlaceHolder>

                <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">

                    <fieldset class="form-horizontal">
                        <legend>Change Password Form</legend>
                        <div class="control-group">
                            <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="control-label">Current password</asp:Label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                    CssClass="text-error" ErrorMessage="The current password field is required."
                                    ValidationGroup="ChangePassword" />
                            </div>
                        </div>
                        <div class="control-group">
                            <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="control-label">New password</asp:Label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                    CssClass="text-error" ErrorMessage="The new password is required."
                                    ValidationGroup="ChangePassword" />
                            </div>
                        </div>
                        <div class="control-group">
                            <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="control-label">Confirm new password</asp:Label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                    CssClass="text-error" Display="Dynamic" ErrorMessage="Confirm new password is required."
                                    ValidationGroup="ChangePassword" />
                                <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                    CssClass="text-error" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                                    ValidationGroup="ChangePassword" />
                            </div>
                        </div>
                        <div class="form-actions no-color">
                            <asp:Button runat="server" Text="Change password" OnClick="ChangePassword_Click" CssClass="btn" ValidationGroup="ChangePassword" />
                        </div>
                    </fieldset>
                </asp:PlaceHolder>
            </section>

            <section id="externalLoginsForm">

                <asp:ListView runat="server"
                    ItemType="Microsoft.AspNet.Identity.IUserLogin"
                    SelectMethod="GetLogins" DeleteMethod="RemoveLogin" DataKeyNames="LoginProvider,ProviderKey">

                    <LayoutTemplate>
                        <h3>Registered Logins</h3>
                        <table class="table">
                            <tbody>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </tbody>
                        </table>

                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#: Item.LoginProvider %></td>
                            <td>
                                <asp:Button runat="server" Text="Remove" CommandName="Delete" CausesValidation="false"
                                    ToolTip='<%# "Remove this " + Item.LoginProvider + " login from your account" %>'
                                    Visible="<%# CanRemoveExternalLogins %>" CssClass="btn" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

                <uc:OpenAuthProviders runat="server" ReturnUrl="~/Account/Manage" />
            </section>

        </div>
    </div>

</asp:Content>
