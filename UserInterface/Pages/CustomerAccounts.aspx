<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAccounts.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.CustomerAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
      <telerik:RadPanelBar runat="server" ID="SearchPanelBar" Width="100%">
        <Items>
          <telerik:RadPanelItem Text="Search" Font-Bold="true">
            <Items>
              <telerik:RadPanelItem>
                <ItemTemplate>
                  <table id="filterGridTable" runat="server">
                    <tr>
                      <td>
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:CompanyCode%>" /></td>
                      <td>
                        <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxBpCode" EmptyMessage="Company code" DataSourceID="autoCompleteBoxBpCode" DataTextField="bpCode" AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxCompanyCode_EntryAdded" OnEntryRemoved="RadAutoCompleteBoxCompanyCode_EntryAdded" InputType="Token">
                          <TextSettings SelectionMode="Single" />
                        </telerik:RadAutoCompleteBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Literal ID="LiteralName" runat="server" Text="<%$Resources:CompanyName%>" /></td>
                      <td>
                        <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxBpName" EmptyMessage="Company name" DataSourceID="autoCompleteBoxBpName" DataTextField="bpName" AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxBpName_EntryAdded" OnEntryRemoved="RadAutoCompleteBoxBpName_EntryAdded" InputType="Token">
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
      <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="-1" DataSourceID="LinqDataSource1" GridLines="Both" GroupPanelPosition="Top" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowMultiRowSelection="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" OnItemCommand="RadGridCertificate_ItemCommand">
        <MasterTableView DataSourceID="LinqDataSource1" AutoGenerateColumns="False">
          <Columns>
            <telerik:GridBoundColumn HeaderText="Business Partner ID" DataField="businessPartnerId" UniqueName="businessPartnerId" ReadOnly="True" Visible="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="bpCode" ReadOnly="True" HeaderText="<%$Resources:CompanyCode%>" SortExpression="bpCode" UniqueName="bpCode" FilterControlAltText="Filter company code column">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="bpName" ReadOnly="True" HeaderText="<%$Resources:CompanyName%>" SortExpression="bpName" UniqueName="bpName" FilterControlAltText="Filter company name column">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="countryCode" ReadOnly="True" HeaderText="<%$Resources:Country%>" SortExpression="countryCode" UniqueName="countryCode" FilterControlAltText="Filter country column">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="User.userSetting.name" ReadOnly="True" HeaderText="Account Manager" SortExpression="accountManager" UniqueName="accountManager" DataType="System.Guid" FilterControlAltText="Filter Account Manager column">
            </telerik:GridBoundColumn>
            <telerik:GridCheckBoxColumn DataField="isMother" ReadOnly="True" HeaderText="Is mother" SortExpression="isMother" UniqueName="isMother" DataType="System.Boolean" FilterControlAltText="Filter is mother column"></telerik:GridCheckBoxColumn>
            <telerik:GridButtonColumn ButtonType="imagebutton" ImageUrl="~/Images/magnifier.png" ConfirmDialogHeight="50px" CommandName="Outsource"></telerik:GridButtonColumn>
          </Columns>
        </MasterTableView>
      </telerik:RadGrid>
      <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (businessPartnerId, company, contactPerson, countryCode, bpName, bpCode, companyCode, country, accountManager, isMother, User)" TableName="businessPartners" OnSelecting="LinqDataSource1_Selecting"></asp:LinqDataSource>
    </ContentTemplate>
  </asp:UpdatePanel>
  <asp:LinqDataSource runat="server" EntityTypeName="" ID="autoCompleteBoxBpName" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (bpName)" TableName="businessPartners" OnSelecting="autoCompleteBox_Selecting"></asp:LinqDataSource>
  <asp:LinqDataSource runat="server" EntityTypeName="" ID="autoCompleteBoxBpCode" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (bpCode)" TableName="businessPartners" OnSelecting="autoCompleteBoxBpCode_Selecting"></asp:LinqDataSource>
</asp:Content>
