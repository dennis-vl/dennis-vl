<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DOPDetails.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.DOP.DOPDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
          <div style="color: #ff0000;  font-size: 12px;    font-weight: bold;   " id="warningDiv" runat="server"> <asp:Literal ID="warningLiteral" runat="server" Text="" Visible="false" /></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<table id="filterGridTable" runat="server">
				<tr>
					<td>
						<asp:Literal ID="LiteralSalesOrder" runat="server" Text="<%$Resources:DeclarationOfPerformance%>" /></td>
					<td></td>
				</tr>
				<tr>
					<td>
						<p>
							<asp:Literal runat="server" Text="<%$Resources:Supplier%>" />
						</p>
					</td>
					<td>
						<telerik:RadTextBox ID="SearchTxtBox" runat="server" EmptyMessage="<%$Resources:Supplier%>" LabelWidth="64px" Resize="None" Width="160px">
						</telerik:RadTextBox>
						<telerik:RadButton ID="SearchButton" runat="server" Text="<%$Resources:Search%>" OnClick="searchButton_Click"></telerik:RadButton>
					</td>
				</tr>
			</table>
			<telerik:RadGrid ID="RadGridCertificate" PageSize="10" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowMultiRowSelection="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" DataSourceID="businessPartnerDocumentsDataSource" CellSpacing="-1" GridLines="Both" OnItemCreated="RadGridCertificate_ItemCreated">
				<AlternatingItemStyle Height="55px" />
				<MasterTableView AutoGenerateColumns="false" DataSourceID="businessPartnerDocumentsDataSource" DataKeyNames="bpName, DOPLink, description, businessPartnerId">

					<Columns>
						<telerik:GridClientSelectColumn UniqueName="ClientSelectionColumn" />
						<telerik:GridBoundColumn DataField="bpName" FilterControlAltText="Filter bpName column" HeaderText="<%$Resources:SupplierName%>" ReadOnly="True" SortExpression="bpName" UniqueName="bpName">
							<ColumnValidationSettings>
								<ModelErrorMessage Text="" />
							</ColumnValidationSettings>
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn DataField="description" FilterControlAltText="Filter description column" HeaderText="<%$Resources:Document%>" ReadOnly="True" SortExpression="description" UniqueName="description">
							<ColumnValidationSettings>
								<ModelErrorMessage Text="" />
							</ColumnValidationSettings>
						</telerik:GridBoundColumn>
						<telerik:GridTemplateColumn ReadOnly="True" HeaderText="<%$Resources:Download%>" UniqueName="DownloadCertificate" AllowFiltering="False">
							<ItemTemplate>
								<asp:HyperLink ID="CertificateHyperLink" runat="server" NavigateUrl='<%#Eval("DOPLink")%>' Target="_blank" Visible='<%# String.IsNullOrEmpty(Convert.ToString(Eval("DOPLink"))) ? false : true %>'>
                                    <img src="../../Images/Adobe_PDF_Icon_svg.png" height="43" alt="Show Certificate" border="0" />
								</asp:HyperLink>
							</ItemTemplate>
						</telerik:GridTemplateColumn>
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
			<asp:LinqDataSource ID="businessPartnerDocumentsDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" OnSelecting="businessPartnerDocumentsDataSource_Selecting" OnSelected="businessPartnerDocumentsDataSource_Selected">
			</asp:LinqDataSource>

			<div id="GridButtonsDiv" style="margin-top: 5px;">
				<telerik:RadButton ID="RadButtonMergePDFs" runat="server" Text="Print" Width="75px" OnClick="RadButtonMergePDFs_Click" />
				<telerik:RadButton ID="RadButtonSelectedInZip" runat="server" Text="Download" Width="75px" OnClick="RadButtonSelectedInZip_Click" />

				<div>
					<asp:Literal ID="LiteralNoCertificatesSelected" runat="server" Text="Nothing selected" Visible="false" />
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
