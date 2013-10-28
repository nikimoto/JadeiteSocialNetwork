<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SocialNetwork.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <header>
        <h1><%: Title %></h1>
    </header>
    <asp:Panel runat="server" CssClass="panel align-center">
        <h2>Contact your bright side.</h2>
        <div class="align-left-margin">
            <address>
                One Teleric Way<br />
                Coruscant, WA 98052-6399<br />
                <abbr title="Phone">P:</abbr>
                425.555.0100
            </address>

            <address>
                <i class="icon-envelope"></i><strong>Jadeite Support:</strong>   <a href="mailto:Support@jadeite.com">Support@jadeite.com</a><br />
                <i class="icon-envelope"></i><strong>Jadeite Marketing:</strong> <a href="mailto:Marketing@jadeite.com">Marketing@jadeite.com</a>
            </address>
        </div>
    </asp:Panel>
</asp:Content>
