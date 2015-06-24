<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectSisters.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.SelectSisters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
      <fieldset>
        <legend>
          <asp:Literal ID="selectLiteral" runat="server" Text="<%$Resources:SelectSisters%>" />
        </legend>
        <table>
          <tr>
            <td>
              <div style="padding-left: 1px;">
                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxBpName" EmptyMessage="Company name" DataSourceID="autoCompleteBoxBpName" DataTextField="bpName" AutoPostBack="True" Width="369px" DropDownWidth="368px" OnEntryAdded="RadAutoCompleteBoxBpName_EntryAdded" OnEntryRemoved="RadAutoCompleteBoxBpName_EntryAdded" InputType="Token">
                  <TextSettings SelectionMode="Single" />
                </telerik:RadAutoCompleteBox>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <telerik:RadListBox runat="server" ID="RadListBoxSource" DataValueField="businessPartnerId" ButtonSettings-ShowTransferAll="false" DataTextField="bpName" Height="300px" Width="400px" TransferMode="Move" AutoPostBackOnTransfer="true"
                AllowTransfer="true" TransferToID="RadListBoxDestination" DataSourceID="LinqDataSourceSrc">
              </telerik:RadListBox>
              <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceSrc" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="businessPartners" OnSelecting="LinqDataSourceSrc_Selecting">
              </asp:LinqDataSource>
            </td>
            <td>
              <telerik:RadListBox runat="server" ID="RadListBoxDestination" DataValueField="businessPartnerId" ButtonSettings-ShowTransferAll="false" DataTextField="bpName" Height="300px" Width="400px" TransferMode="Move" TransferToID="RadListBoxSource" AllowDelete="true" DataSourceID="LinqDataSourceDest" AutoPostBack="true">
              </telerik:RadListBox>
              <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceDest" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="businessPartners" OnSelecting="LinqDataSourceDest_Selecting">
              </asp:LinqDataSource>
            </td>
            <tr>
              <td>
                <telerik:RadButton ID="submitButton" runat="server" Text="Save" OnClick="submitButton_Click"></telerik:RadButton>
                <telerik:RadButton ID="cancelButton" runat="server" Text="Cancel" OnClick="cancelButton_Click"></telerik:RadButton>
              </td>
            </tr>
          </tr>
          <asp:LinqDataSource runat="server" EntityTypeName="" ID="autoCompleteBoxBpName" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="businessPartners" OnSelecting="autoCompleteBoxBpName_Selecting">
          </asp:LinqDataSource>
        </table>
      </fieldset>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
