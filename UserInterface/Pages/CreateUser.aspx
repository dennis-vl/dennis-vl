<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.CreateUser" %>
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
                    <asp:Literal ID="LiteralCreateAdministrator" runat="server" Text="Create user" />
                </legend>
                <table id="createAdminTable">
                    <tr>
                        <td>
                            <asp:Label ID="LabelName" runat="server" Text="<%$Resources:Name%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelUserName" runat="server" Text="Username" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxUserName" runat="server" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="Username is not valid." ControlToValidate="TextBoxUserName" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserName" runat="server"  ErrorMessage="Username is required." ControlToValidate="TextBoxUserName" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="companyLbl" runat="server" Text="<%$Resources:Company%>" />
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="companyDDL" DataTextField="companyName" DataValueField="companyCode" runat="server" DataSourceID="LinqDataSource1"></telerik:RadDropDownList>
                            <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (companyCode, companyName)" TableName="companies"></asp:LinqDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelPassword" runat="server" Text="<%$Resources:Password%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ErrorMessage="Password is required." ControlToValidate="TextBoxPassword" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPassword" runat="server" ErrorMessage="Password must be at least 6 characters with at least one Capital letter, at least one lower case letter and at least one number." ControlToValidate="TextBoxPassword" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelConfirmPassword" runat="server" Text="<%$Resources:ConfirmPassword%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorConfirmPassword" runat="server" ErrorMessage="Repeat new password." ControlToValidate="TextBoxConfirmPassword" />
                            <asp:CompareValidator ID="CompareValidatorPassword" runat="server" ErrorMessage="Passwords do not match." ControlToValidate="TextBoxConfirmPassword" ControlToCompare="TextBoxPassword" />
                        </td>
                    </tr>
                    
                </table>
                <div id="forgotPasswordDiv">
                        <telerik:RadButton ID="RadButton1" runat="server" Text="Create" OnClick="RadButtonCreate_Click" />
                </div>
                </fieldset>
        </asp:PlaceHolder>
    </section>
</asp:Content>
