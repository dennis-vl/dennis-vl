<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemDetail.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm.ItemDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HyperLink ID="HyperLinkItemOverview" runat="server" NavigateUrl="~/Pages/Esm/ItemOverview.aspx" Text="Back to item overview" />
    <telerik:RadListView ID="RadListViewItem" runat="server" DataSourceID="LinqDataSourceRadListViewItem" ItemPlaceholderID="PanelItemPlaceholder" AllowPaging="true">
        <LayoutTemplate>
            <fieldset>
                <legend>Article detail</legend>
                <div>
                    <telerik:RadDataPager ID="RadDataPagerItem" runat="server" PageSize="1" OnPageIndexChanged="RadDataPagerItem_PageIndexChanged">
                        <Fields>
                            <telerik:RadDataPagerButtonField FieldType="FirstPrev"></telerik:RadDataPagerButtonField>
                            <telerik:RadDataPagerButtonField FieldType="Numeric" PageButtonCount="5"></telerik:RadDataPagerButtonField>
                            <telerik:RadDataPagerButtonField FieldType="NextLast"></telerik:RadDataPagerButtonField>
                            <telerik:RadDataPagerTemplatePageField>
                                <PagerTemplate>
                                    <div style="float: right">
                                        Items
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.Owner.StartRowIndex+1%>" />
                                        to
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Container.Owner.TotalRowCount > (Container.Owner.StartRowIndex+Container.Owner.PageSize) ? Container.Owner.StartRowIndex+Container.Owner.PageSize : Container.Owner.TotalRowCount %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.Owner.TotalRowCount%>" />
                                    </div>
                                </PagerTemplate>
                            </telerik:RadDataPagerTemplatePageField>
                        </Fields>
                    </telerik:RadDataPager>
                </div>
                <asp:Panel ID="PanelItemPlaceholder" runat="server">
                </asp:Panel>
            </fieldset>
        </LayoutTemplate>

        <ItemTemplate>
            <div id="itemDetailItem">
                <fieldset>
                    <legend>Article</legend>
                    <table>
                        <tr>
                            <td>
                                Size:
                            </td>
                            <td>
                                <%# Eval("size") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Article description:
                            </td>
                            <td>
                                <%# Eval("description") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Article group:
                            </td>
                            <td>
                                <%# Eval("itemGroupName") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Article code:
                            </td>
                            <td>
                                <%# Eval("itemCode") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Company:
                            </td>
                            <td>
                                <%# Eval("companyName") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Weight:
                            </td>
                            <td>
                                <%# Eval("weight") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Stock available:
                            </td>
                            <td>
                                <%# Eval("stockAvailable") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Stock on order:
                            </td>
                            <td>
                                <%# Eval("purchaseOpenQuantity") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last update:
                            </td>
                            <td>
                                <%# Eval("timeStampLastUpdate") %>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>Pricing</legend>
                </fieldset>
            </div>
        </ItemTemplate>
    </telerik:RadListView>

    <div id="itemDetailPurchase">
        <fieldset>
            <legend>Purchase order</legend>
            <telerik:RadGrid ID="RadGridPurchaseOrder" runat="server" DataSourceID="LinqDataSourceRadGridDataSource">
                <MasterTableView DataSourceID="LinqDataSourceRadGridDataSource" AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridBoundColumn DataField="openQuantity" ReadOnly="True" HeaderText="Stock" UniqueName="openQuantity">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="uomCode" ReadOnly="True" HeaderText="UOM" UniqueName="uomCode">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="deliveryDate" ReadOnly="True" HeaderText="Delivery date" UniqueName="deliveryDate">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="bpName" ReadOnly="True" HeaderText="Business partner" UniqueName="bpName">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="description" ReadOnly="True" HeaderText="Country" UniqueName="description">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </div>

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceRadListViewItem" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (size, description, itemGroupName, companyName, itemCode, weight, timeStampLastUpdate, purchaseOpenQuantity, stockAvailable)" TableName="vw_items" OnSelecting="LinqDataSourceRadListViewItem_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceRadGridDataSource" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" OnSelecting="LinqDataSourceRadGridDataSource_Selecting" />
</asp:Content>
