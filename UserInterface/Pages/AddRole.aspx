<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRole.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.AddRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
            <fieldset>           
                <legend>
                    <asp:Literal ID="Literal1" runat="server" Text="User account" />
                </legend>
                <table id="userTable">
                 <tr>
                    <td style="width: 220px;">
                        <asp:Literal ID="UsernameLiteral" text="Username" runat="server"/>
                    </td>
                    <td>
                        <asp:Literal ID="UsernameLiteral2" text="" runat="server"/>
                    </td>
                </tr>
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
                            <asp:Label ID="companyLbl" runat="server" Text="<%$Resources:Company%>" />
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="companyDDL" DataTextField="companyName" DataValueField="companyCode" runat="server" DataSourceID="LinqDataSource1"></telerik:RadDropDownList>
                            <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (companyCode, companyName)" TableName="companies"></asp:LinqDataSource>
                        </td>
                </tr>
                </table>
                <div id="adminCheck" style="float: right;  margin-right: 0; margin-top: -85px;">
                    <p id="adminText" style="float: right;  margin-top: -1px; padding-left: 5px;">Administrator </p> <asp:CheckBox ID="adminCheckBox" runat="server" CssClass="floatRight" />
                </div>
             </fieldset>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset>
                    <legend>
                        <asp:Literal ID="Literal2" runat="server" Text="Applications" />
                    </legend>
                    <table id="adminRolesTable1">
                    <tr>
                        <telerik:RadListBox ID="RadListBoxApplications" runat="server"  DataSourceID="applicationsDataSource" DataTextField="applicationName" DataKeyField="applicationCode"  AutoPostBack="True" OnSelectedIndexChanged="RadListBoxApplications_SelectedIndexChanged" ></telerik:RadListBox>
                        <asp:LinqDataSource ID="applicationsDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" EntityTypeName="" Select="new (applicationName, applicationCode)" TableName="applications" EnableUpdate="True">
			            </asp:LinqDataSource>
                    </tr>
                    </table>

                <fieldset>
                <legend>
                    <asp:Literal ID="rolesLiteral" runat="server" Text="Roles" />
                </legend>
                <table id="adminRolesTable2">
                    <tr>
                        <td>
                            <telerik:RadListBox runat="server" ID="RadListBoxSource" Height="200px" Width="200px"  TransferMode="Copy" AutoPostBackOnTransfer="true"   AllowTransferDuplicates="false"
                                AllowTransfer="true" TransferToID="RadListBoxDestination" DataSourceID="rolesDataSource" DataTextField="roleCode">
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadListBox runat="server" ID="RadListBoxDestination" Height="200px" Width="200px" AllowTransfer="false" TransferMode="Copy" AllowTransferDuplicates="false" TransferToID="RadListBoxSource" AllowDelete="true"  DataSourceID="rolesDestinationDataSource">
                            </telerik:RadListBox>
                            <asp:LinqDataSource runat="server" EntityTypeName="" OnSelecting="rolesDestinationDataSource_Selecting" ID="rolesDestinationDataSource"></asp:LinqDataSource>
                        </td>
                    </tr>
                </table>
            <asp:LinqDataSource ID="rolesDataSource" runat="server" OnSelecting="rolesDataSource_Selecting" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" EntityTypeName="" TableName="applicationRoles" Select="new (roleCode, applicationCode, description, userRoles, application)" Where="applicationCode == @applicationCode">
            </asp:LinqDataSource>

            <div id="forgotPasswordDiv">
                <telerik:RadButton ID="RadButtonCreate" runat="server" Text="<%$Resources:Submit%>" OnClick="RadButtonCreate_Click" /> <telerik:RadButton ID="RadButton1" runat="server" Text="<%$Resources:Cancel%>"  OnClick="RadButton1_Click" />
            </div>
            </fieldset>
         </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
