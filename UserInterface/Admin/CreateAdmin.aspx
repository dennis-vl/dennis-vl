<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateAdmin.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Admin.CreateAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <section id="createAdminSection">
        
        <asp:Label ID="CreateAdminStatus" runat="server" Text="" />

        <asp:PlaceHolder runat="server" ID="PlaceHolderCreateAdmin">
            <fieldset>
                <legend>
                    <asp:Literal ID="LiteralCreateAdministrator" runat="server" Text="<%$Resources:CreateAdministrator%>" />
                </legend>
                <table id="createAdminTable">
                    <tr>
                        <td>
                            <asp:Label ID="LabelUserName" runat="server" Text="<%$Resources:Username%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxUserName" runat="server" />
                          <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="<%$Resources:UsernameNotValid%>" ControlToValidate="TextBoxUserName" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserName" runat="server" ErrorMessage="<%$Resources:UsernameIsRequired%>" ControlToValidate="TextBoxUserName" />
                        --%></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelPassword" runat="server" Text="<%$Resources:Password%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ErrorMessage="<%$Resources:PasswordIsRequired%>" ControlToValidate="TextBoxPassword" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPassword" runat="server" ErrorMessage="Password must be at least 6 characters with at least one Capital letter, at least one lower case letter and at least one number." ControlToValidate="TextBoxPassword" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" />
                        --%></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelConfirmPassword" runat="server" Text="<%$Resources:ConfirmPassword%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password" />
                          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidatorConfirmPassword" runat="server" ErrorMessage="<%$Resources:RepeatNewPasswordIsRequired%>" ControlToValidate="TextBoxConfirmPassword" />
                            <asp:CompareValidator ID="CompareValidatorPassword" runat="server" ErrorMessage="<%$Resources:PasswordDoesNotMatch%>" ControlToValidate="TextBoxConfirmPassword" ControlToCompare="TextBoxPassword" />
                        --%></td>
                    </tr>
                </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Literal ID="rolesLiteral" runat="server" Text="Roles" />
                    </legend>
                    <table id="adminRolesTable">
                    <tr>
                        <telerik:RadListBox ID="RadListBoxApplications" runat="server" DataSourceID="applicationsDataSource" DataTextField="applicationName" DataKeyField="applicationCode" OnSelectedIndexChanged="RadListBoxApplications_SelectedIndexChanged1" AutoPostBack="True" ></telerik:RadListBox>
                        <asp:LinqDataSource ID="applicationsDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" EntityTypeName="" Select="new (applicationName, applicationCode)" TableName="applications" EnableUpdate="True">
			            </asp:LinqDataSource>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox runat="server" ID="RadListBoxSource" Height="200px" Width="230px"
                                AllowTransfer="true" TransferToID="RadListBoxDestination" DataSourceID="rolesDataSource" DataTextField="roleCode">
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadListBox runat="server" ID="RadListBoxDestination" Height="200px" Width="200px" >
                            </telerik:RadListBox>
                        </td>
                        <asp:LinqDataSource ID="rolesDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" EntityTypeName="" TableName="applicationRoles" Select="new (roleCode, applicationCode, description, userRoles, application)" Where="applicationCode == @applicationCode">
                            
                        </asp:LinqDataSource>
                    </tr>
                </table>
                <div id="forgotPasswordDiv">
                    <telerik:RadButton ID="RadButtonCreate" runat="server" Text="<%$Resources:Create%>" OnClick="RadButtonCreate_Click" />
                </div>
                </fieldset>
        </asp:PlaceHolder>
    </section>
</asp:Content>
