<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAccounts.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.UserAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">
        function NoCharactersAfterOneEntryBlock(sender, eventArgs) {
            var autoCompleteBox = sender;
            var firstEntry = autoCompleteBox.get_entries().getEntry(0);

            if (jQuery.browser.version >= 10) {

                if (firstEntry) {
                    autoCompleteBox.get_entries().clear();
                    autoCompleteBox.clear();
                    autoCompleteBox.get_entries().add(firstEntry);

                    eventArgs.set_cancel(true);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">  
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
            <telerik:RadPanelBar runat="server" ID="SearchPanelBar" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Search" Font-Bold="true" >
                        <Items>
                            <telerik:RadPanelItem>
                                <ItemTemplate>
                                    <table ID="filterGridTable" runat="server">
                                        <tr>
                                            <td><asp:Literal ID="LiteralName" runat="server" Text="<%$Resources:Name%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxName" EmptyMessage="Name" DataSourceID="AutoCompleteBoxNameDataSource" DataTextField="name"  AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxName_EntryAdded"  OnEntryRemoved="RadAutoCompleteBoxName_EntryAdded" InputType="Token">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralUserName" runat="server" Text="Username" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxUserName" EmptyMessage="Username" DataSourceID="AutoCompleteBoxUserNameDataSource" DataTextField="UserName"  AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxUserName_EntryAdded"  OnEntryRemoved="RadAutoCompleteBoxUserName_EntryAdded" InputType="Token">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadPanelItem>
                        </Items>
                        
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>


            <telerik:RadGrid ID="RadGridCertificate" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowMultiRowSelection="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" CellSpacing="-1" GridLines="Both" DataSourceID="DataSourceLinq" OnItemCommand="RadGridCertificate_ItemCommand">
                <AlternatingItemStyle Height="55px" />
                <MasterTableView AutoGenerateColumns="false" DataSourceID="DataSourceLinq">
                    <Columns> 
                        <telerik:GridBoundColumn DataField="name" ReadOnly="True" HeaderText="<%$Resources:Name%>" SortExpression="name" UniqueName="name" FilterControlAltText="Filter name column">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn FilterControlAltText="Filter UserName column" HeaderText="Username" ReadOnly="True" DataField="UserName" SortExpression="UserName" UniqueName="UserName">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="imagebutton"  CommandName="Outsource"  ImageUrl="~/Images/magnifier.png" ConfirmDialogHeight="50px" ></telerik:GridButtonColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
                <ItemStyle Height="55px" />
                <PagerStyle AlwaysVisible="true" />
                <ExportSettings>
                    <Pdf PageWidth="">
                    </Pdf>
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="True" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadButton ID="createAccount" runat="server" Text="<%$Resources:CreateUser%>" OnClick="createAccount_Click"></telerik:RadButton>
            <asp:LinqDataSource runat="server" EntityTypeName="" ID="DataSourceLinq" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (UserName, name, UserId)" TableName="vw_name_in_users" OnSelecting="DataSourceLinq_Selecting"></asp:LinqDataSource>

        </ContentTemplate>
	</asp:UpdatePanel>
    <asp:LinqDataSource ID="AutoCompleteBoxNameDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_name_in_users" Select="new (name)" OnSelecting="AutoCompleteBoxNameDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxUserNameDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_name_in_users" Select="new (UserName)" OnSelecting="AutoCompleteBoxUserNameDataSource_Selecting" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
