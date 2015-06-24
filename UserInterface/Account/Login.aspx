<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Account.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    <section id="loginSection">
        <asp:Login runat="server" ID="LoginBox" ViewStateMode="Disabled" RenderOuterTable="False" DestinationPageUrl="~/Pages/DeliveryDetails.aspx" FailureText="<%$Resources:LoginFailed%>" OnLoggedIn="LoginBox_LoggedIn">
            <LayoutTemplate>
                <p>
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend><asp:Literal runat="server" ID="LiteralLogIn" Text="<%$Resources:LogIn%>" /></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" AssociatedControlID="UserName" Text="<%$Resources:Username%>" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="UserName" Width="150px" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" ErrorMessage="<%$Resources:UsernameIsRequired%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelPassword" runat="server" AssociatedControlID="Password" Text="<%$Resources:Password%>" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" Width="150px" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" ErrorMessage="<%$Resources:PasswordIsRequired%>" />
                            </td>
                        </tr>
                    </table>
                    <div id="loginRememberMe">
                        <asp:CheckBox runat="server" ID="CheckBoxRememberMe" />
                        <asp:Label ID="RememberMe" runat="server" AssociatedControlID="RememberMe" Text="<%$Resources:RememberMe%>" />
                    </div>
                    <telerik:RadButton ID="RadButtonLogin" runat="server" CommandName="Login" Text="<%$Resources:LogIn%>" />
                    <br/><br/>
                    <asp:HyperLink ID="HyperLinkForgetPassword" runat="server" NavigateUrl="~/Account/ForgetPassword.aspx" Text="<%$Resources:ForgotPassword%>" />
                    <asp:HyperLink ID="HyperLinkRegister" runat="server" NavigateUrl="~/Account/Register.aspx" Text="<%$Resources:GenerateAccount%>" />
                
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
        <div id="thawteseal" style="text-align:center;" title="Click to Verify - This site chose Thawte SSL for secure e-commerce and confidential communications.">
            <div>
                <script type="text/javascript" src="https://seal.thawte.com/getthawteseal?host_name=portal.vanleeuwen.com&amp;size=M&amp;lang=en">
                </script>
            </div>
            <div>
                <a href="http://www.thawte.com/ssl-certificates/" target="_blank" style="color:#000000; text-decoration:none; font:bold 10px arial,sans-serif; margin:0px; padding:0px;">
                    ABOUT SSL CERTIFICATES
                </a>
            </div>
        </div>
    </section>
    
</asp:Content>
