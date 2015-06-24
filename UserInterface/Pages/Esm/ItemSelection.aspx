<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemSelection.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm.ItemSelection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadGrid ID="RadGridItemSelection" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="LinqDataSourceRadGrid" GridLines="None" AllowMultiRowSelection="True" AllowFilteringByColumn="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" OnItemCreated="RadGridItemOverview_ItemCreated">
        <AlternatingItemStyle Height="55px" />
        <MasterTableView AutoGenerateColumns="false" DataSourceID="LinqDataSourceRadGrid" DataKeyNames="itemId, description, size, itemGroupName, companyName, stockAvailable, grossPrice, mAvPrice, purchaseOpenQuantity, weight, itemCode, companyCode">

            <GroupByExpressions>
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="Description" FieldName="description" />
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="itemId" />
                    </GroupByFields>
                </telerik:GridGroupByExpression>
            </GroupByExpressions>

            <Columns>
                <telerik:GridClientSelectColumn UniqueName="gridClientSelectColumn" />

                <telerik:GridBoundColumn DataField="size" ReadOnly="True" HeaderText="Size" UniqueName="size" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="itemGroupName" ReadOnly="True" HeaderText="Article group" UniqueName="itemGroupName" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="companyName" ReadOnly="True" HeaderText="Company" UniqueName="companyName" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="stockAvailable" ReadOnly="True" HeaderText="Stock available" UniqueName="stockAvailable" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="grossPrice" ReadOnly="True" HeaderText="Gross price" UniqueName="grossPrice" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="mAvPrice" ReadOnly="True" HeaderText="Moving average price" UniqueName="mAvPrice" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="purchaseOpenQuantity" ReadOnly="True" HeaderText="On order" UniqueName="purchaseOpenQuantity" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="weight" ReadOnly="True" HeaderText="KG" UniqueName="weight" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="itemCode" ReadOnly="True" HeaderText="Article number" UniqueName="itemCode" AllowFiltering="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""/>
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn HeaderText="Detail" AllowFiltering="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButtonDetail" runat="server" OnClick="ImageButtonDetail_Click" ImageUrl="~/Images/magnifier.png" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>

        </MasterTableView>
        <ItemStyle Height="55px" />
        <PagerStyle AlwaysVisible="true" />
        <ExportSettings>
            <Pdf PageWidth="" />
        </ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="True" />
        </ClientSettings>
    </telerik:RadGrid>

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceRadGrid" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (itemId, description, size, itemGroupName, companyName, stockAvailable, grossPrice, mAvPrice, purchaseOpenQuantity, weight, itemCode, companyCode)" TableName="vw_items" OnSelecting="LinqDataSourceRadGrid_Selecting" />

</asp:Content>
