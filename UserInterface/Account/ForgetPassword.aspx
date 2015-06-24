<%@ Page Title="Forget Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Account.ForgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <section id="forgotPasswordSection">
        <%--<asp:PasswordRecovery ID="PasswordRecovery1" runat="server">

            <UserNameTemplate>
                <p>
                    <!-- this Literal id must be FailureText -->
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
                </p>
                <fieldset>
                    <legend>Enter your username to receive your password</legend>
                    <table id="forgotPasswordTable">
                        <tr>
                            <td>
                                <asp:Label ID="LabelUserName" runat="server" Text="Username:" />
                            </td>
                            <td>
                                <!-- this TextBox id must be UserName -->
                                <asp:TextBox ID="UserName" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserName" runat="server" ErrorMessage="Username is required" ControlToValidate="UserName" />
                            </td>
                        </tr>
                    </table>
                    <div id="forgotPasswordDiv">
                        <telerik:RadButton ID="RadButtonSend" runat="server" Text="Send" CommandName="Submit" />
                        <asp:HyperLink ID="HyperLinkCancel" runat="server" NavigateUrl="~/Account/Login.aspx">Cancel</asp:HyperLink>
                    </div>
                </fieldset>
            </UserNameTemplate>

        </asp:PasswordRecovery>--%>

        <p id="forgotPasswordUsernameNotFound">
            <asp:Literal ID="LiteralUsernameNotFound" runat="server" EnableViewState="False" Visible="false" Text="<%$Resources:UsernameNotFound%>" />
        </p>
        <p id="forgotPasswordRequestSuccesfull">
            <asp:Literal ID="LiteralPasswordRequestSuccesfull" runat="server" EnableViewState="False" Visible="false" Text="<%$Resources:PasswordRequestSuccesfull%>" />
        </p>
        <asp:Panel ID="PanelForgetPassword" runat="server">
            <fieldset>
                <legend><asp:Literal runat="server" ID="LiteralEnterUsernameForPassword" Text="<%$Resources:EnterUsernameForPassword%>" /></legend>
                <table id="forgotPasswordTable">
                    <tr>
                        <td>
                            <asp:Label ID="LabelUserName" runat="server" Text="<%$Resources:Username%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxUserName" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserName" runat="server" ErrorMessage="<%$Resources:UsernameIsRequired%>" ControlToValidate="TextBoxUserName" />
                        </td>
                    </tr>
                </table>
                <div id="forgotPasswordDiv">
                    <telerik:RadButton ID="RadButtonSend" runat="server" Text="<%$Resources:Send%>" OnClick="RadButtonSend_Click" />
                    <asp:HyperLink ID="HyperLinkCancel" runat="server" NavigateUrl="~/Account/Login.aspx" Text="<%$Resources:Cancel%>" />
                </div>
            </fieldset>
        </asp:Panel>
    </section>
</asp:Content>
