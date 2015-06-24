<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Account.Manage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <section id="manageSection">
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p><%: SuccessMessage %></p>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="changePassword">
            
            <asp:ChangePassword runat="server" CancelDestinationPageUrl="~/" ViewStateMode="Disabled" RenderOuterTable="false" SuccessPageUrl="Manage?m=ChangePwdSuccess">
                <ChangePasswordTemplate>
                    <p>
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                    <fieldset>
                        <legend><asp:Literal runat="server" ID="LiteralChangePasswordOf" Text="<%$Resources:ChangePasswordOf%>" />&nbsp;<asp:LoginName ID="LoginNameManage" runat="server" />:</legend>
                        <table id="manageTable">
                            <tr>
                                <td class="manageTdWidth">
                                    <asp:Literal runat="server" ID="LiteralCurrentPassword" Text="<%$Resources:CurrentPassword%>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" Width="150px" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword" ErrorMessage="<%$Resources:CurrentPasswordIsRequired%>" ValidationGroup="ChangePassword" />
                                </td>
                            </tr>
                            <tr>
                                <td class="manageTdWidth">
                                    <asp:Literal runat="server" ID="LiteralNewPassword" Text="<%$Resources:NewPassword%>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" Width="150px" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword" ErrorMessage="<%$Resources:NewPasswordIsRequired%>" ValidationGroup="ChangePassword" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorNewPassword" runat="server" ErrorMessage="<%$Resources:PasswordRequirement%>" ControlToValidate="NewPassword" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" />
                                </td>
                            </tr>
                            <tr>
                                <td class="manageTdWidth">
                                    <asp:Literal runat="server" ID="LiteralRepeatNewPassword" Text="<%$Resources:RepeatNewPassword%>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" Width="150px" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="<%$Resources:RepeatNewPasswordIsRequired%>" ValidationGroup="ChangePassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="<%$Resources:PasswordDoesNotMatch%>" ValidationGroup="ChangePassword" />
                                </td>
                            </tr>
                        </table>
                        <telerik:RadButton ID="RadButtonChangePassword" runat="server" CommandName="ChangePassword" Text="<%$Resources:Change%>" ValidationGroup="ChangePassword" />
                    </fieldset>
                </ChangePasswordTemplate>
            </asp:ChangePassword>
        </asp:PlaceHolder>
    </section>

</asp:Content>
