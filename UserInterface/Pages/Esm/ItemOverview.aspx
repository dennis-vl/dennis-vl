<%@ Page Title="Articles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemOverview.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm.ItemOverview" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadPanelBar runat="server" ID="RadPanelBarSearch" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="<%$Resources:Search%>">
                        <Items>
                            <telerik:RadPanelItem>
                                <ItemTemplate>
                                    <table ID="filterGridTableSearch" runat="server">
                                        <tr>
                                            <td><asp:Literal ID="LiteralSize" runat="server" Text="<%$Resources:Size%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxSize" EmptyMessage='<%$Resources:Size%>' DataSourceID="LinqDataSourceAcbSize" DataTextField="size" AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxSize_Entry" OnEntryRemoved="RadAutoCompleteBoxSize_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralCompany" runat="server" Text="<%$Resources:Company%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxCompany" EmptyMessage="<%$Resources:Company%>" DataSourceID="LinqDataSourceAcbCompany" DataTextField="companyName" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxCompany_Entry" OnEntryRemoved="RadAutoCompleteBoxCompany_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralDescription" runat="server" Text="<%$Resources:Description%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxDescription" EmptyMessage="<%$Resources:Description%>" DataSourceID="LinqDataSourceAcbDescription" DataTextField="description" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxDescription_Entry" OnEntryRemoved="RadAutoCompleteBoxDescription_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralArticleGroup" runat="server" Text="<%$Resources:ArticleGroup%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxArticleGroup" EmptyMessage="<%$Resources:ArticleGroup%>" DataSourceID="LinqDataSourceAcbGroup" DataTextField="itemGroupName" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxArticleGroup_Entry" OnEntryRemoved="RadAutoCompleteBoxArticleGroup_Entry" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadPanelItem>
                        </Items>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>

            <telerik:RadPanelBar runat="server" ID="RadPanelBarVLunar" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="<%$Resources:VLunar%>">
                        <Items>
                            <telerik:RadPanelItem>
                                <ItemTemplate>
                                    <table ID="filterGridTableVLunar" runat="server">
                                        <tr>
                                            <td><asp:Literal ID="LiteralOutsideDiameter1" runat="server" Text="<%$Resources:OutsideDiameter1%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxOutsideDiameter1" EmptyMessage='<%$ Resources:OutsideDiameter1 %>' DataSourceID="LinqDataSourceAcbOutsideDiameter1" DataTextField="outsideDiameter1" AutoPostBack="True" Width="400px" DropDownWidth="400px" OnEntryAdded="RadAutoCompleteBoxOutsideDiameter1_Entry" OnEntryRemoved="RadAutoCompleteBoxOutsideDiameter1_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralTreatmentHeat" runat="server" Text="<%$Resources:TreatmentHeat%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxTreatmentHeat" EmptyMessage="<%$Resources:TreatmentHeat%>" DataSourceID="LinqDataSourceAcbTreatmentHeat" DataTextField="treatmentHeat" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxTreatmentHeat_Entry" OnEntryRemoved="RadAutoCompleteBoxTreatmentHeat_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralOutsideDiameter2" runat="server" Text="<%$Resources:OutsideDiameter2%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxOutsideDiameter2" EmptyMessage="<%$Resources:OutsideDiameter2%>" DataSourceID="LinqDataSourceAcbOutsideDiameter2" DataTextField="outsideDiameter2" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxOutsideDiameter2_Entry" OnEntryRemoved="RadAutoCompleteBoxOutsideDiameter2_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralTreatmentSurface" runat="server" Text="<%$Resources:TreatmentSurface%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxTreatmentSurface" EmptyMessage="<%$Resources:TreatmentSurface%>" DataSourceID="LinqDataSourceAcbTreatmentSurface" DataTextField="treatmentSurface" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxTreatmentSurface_Entry" OnEntryRemoved="RadAutoCompleteBoxTreatmentSurface_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralOutsideDiameter3" runat="server" Text="<%$Resources:OutsideDiameter3%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxOutsideDiameter3" EmptyMessage="<%$Resources:OutsideDiameter3%>" DataSourceID="LinqDataSourceAcbOutsideDiameter3" DataTextField="outsideDiameter3" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxOutsideDiameter3_Entry" OnEntryRemoved="RadAutoCompleteBoxOutsideDiameter3_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralEnds" runat="server" Text="<%$Resources:Ends%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxEnds" EmptyMessage="<%$Resources:Ends%>" DataSourceID="LinqDataSourceAcbEnds" DataTextField="ends" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxEnds_Entry" OnEntryRemoved="RadAutoCompleteBoxEnds_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralWallThickness1" runat="server" Text="<%$Resources:WallThickness1%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxWallThickness1" EmptyMessage="<%$Resources:WallThickness1%>" DataSourceID="LinqDataSourceAcbWallThickness1" DataTextField="wallThickness1" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxWallThickness1_Entry" OnEntryRemoved="RadAutoCompleteBoxWallThickness1_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralCdi" runat="server" Text="<%$Resources:Cdi%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxCdi" EmptyMessage="<%$Resources:Cdi%>" DataSourceID="LinqDataSourceAcbCdi" DataTextField="cdi" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxCdi_Entry" OnEntryRemoved="RadAutoCompleteBoxCdi_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralWallThickness2" runat="server" Text="<%$Resources:WallThickness2%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxWallThickness2" EmptyMessage="<%$Resources:WallThickness2%>" DataSourceID="LinqDataSourceAcbWallThickness2" DataTextField="wallThickness2" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxWallThickness2_Entry" OnEntryRemoved="RadAutoCompleteBoxWallThickness2_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralSupplier" runat="server" Text="<%$Resources:Supplier%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxSupplier" EmptyMessage="<%$Resources:Supplier%>" DataSourceID="LinqDataSourceAcbSupplier" DataTextField="supplier" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxSupplier_Entry" OnEntryRemoved="RadAutoCompleteBoxSupplier_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralType" runat="server" Text="<%$Resources:Type%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxType" EmptyMessage="<%$Resources:Type%>" DataSourceID="LinqDataSourceAcbType" DataTextField="type" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxType_Entry" OnEntryRemoved="RadAutoCompleteBoxType_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralOther" runat="server" Text="<%$Resources:Other%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxOther" EmptyMessage="<%$Resources:Other%>" DataSourceID="LinqDataSourceAcbOther" DataTextField="other" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxOther_Entry" OnEntryRemoved="RadAutoCompleteBoxOther_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralSpecification" runat="server" Text="<%$Resources:Specification%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxSpecification" EmptyMessage="<%$Resources:Specification%>" DataSourceID="LinqDataSourceAcbSpecification" DataTextField="specification" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxSpecification_Entry" OnEntryRemoved="RadAutoCompleteBoxSpecification_Entry" />
                                            </td>
                                            <td><asp:Literal ID="LiteralCertificates" runat="server" Text="<%$Resources:Certificates%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxCertificates" EmptyMessage="<%$Resources:Certificates%>" DataSourceID="LinqDataSourceAcbCertificates" DataTextField="certificates" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxCertificates_Entry" OnEntryRemoved="RadAutoCompleteBoxCertificates_Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralLength" runat="server" Text="<%$Resources:Length%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxLength" EmptyMessage="<%$Resources:Length%>" DataSourceID="LinqDataSourceAcbLength" DataTextField="length" InputType="Token" AutoPostBack="True" Width="400" DropDownWidth="400" OnEntryAdded="RadAutoCompleteBoxLength_Entry" OnEntryRemoved="RadAutoCompleteBoxLength_Entry" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadPanelItem>
                        </Items>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            
            <telerik:RadGrid ID="RadGridItemOverview" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="LinqDataSourceRadGrid" GridLines="None" AllowMultiRowSelection="True" AllowFilteringByColumn="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" OnItemCreated="RadGridItemOverview_ItemCreated">
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
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="gridButtonsDiv">
        <telerik:RadButton ID="RadButtonSelection" runat="server" Text="Select/Deselect" OnClick="RadButtonSelection_Click" />
        
        <asp:HyperLink ID="HyperLinkShoppingCart" runat="server" Text="Shopping cart" NavigateUrl="~/Pages/Esm/ItemSelection.aspx" />

        <div>
            <asp:Literal ID="LiteralNoRowsSelected" runat="server" Text="You don't have any rows selected" Visible="false" />
        </div>
    </div>

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbSize" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (size)" TableName="vw_items" OnSelecting="LinqDataSourceAcbSize_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbCompany" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (companyName)" TableName="vw_items" OnSelecting="LinqDataSourceAcbCompanyName_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbDescription" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (description)" TableName="vw_items" OnSelecting="LinqDataSourceAcbDescription_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbGroup" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (itemGroupName)" TableName="vw_items" OnSelecting="LinqDataSourceAcbItemGroupName_Selecting" />

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbOutsideDiameter1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (outsideDiameter1)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOutsideDiameter1_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbTreatmentHeat" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (treatmentHeat)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOutsideTreatmentHeat_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbOutsideDiameter2" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (outsideDiameter2)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOutsideDiameter2_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbTreatmentSurface" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (treatmentSurface)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOutsideTreatmentSurface_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbOutsideDiameter3" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (OutsideDiameter3)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOutsideDiameter3_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbEnds" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (ends)" TableName="vw_items" OnSelecting="LinqDataSourceAcbEnds_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbWallThickness1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (wallThickness1)" TableName="vw_items" OnSelecting="LinqDataSourceAcbWallThickness1_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbCdi" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (cdi)" TableName="vw_items" OnSelecting="LinqDataSourceAcbCdi_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbWallThickness2" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (wallThickness2)" TableName="vw_items" OnSelecting="LinqDataSourceAcbWallThickness2_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbSupplier" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (supplier)" TableName="vw_items" OnSelecting="LinqDataSourceAcbSupplier_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbType" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (type)" TableName="vw_items" OnSelecting="LinqDataSourceAcbType_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbOther" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (other)" TableName="vw_items" OnSelecting="LinqDataSourceAcbOther_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbSpecification" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (specification)" TableName="vw_items" OnSelecting="LinqDataSourceAcbSpecification_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbCertificates" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (certificates)" TableName="vw_items" OnSelecting="LinqDataSourceAcbCertificates_Selecting" />
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceAcbLength" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (length)" TableName="vw_items" OnSelecting="LinqDataSourceAcbLength_Selecting" />
    
    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSourceRadGrid" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.DALPortalDataContext" Select="new (itemId, description, size, itemGroupName, companyName, stockAvailable, grossPrice, mAvPrice, purchaseOpenQuantity, weight, itemCode, companyCode)" TableName="vw_items" OnSelecting="DataSourceRadGrid_Selecting" />
    
</asp:Content>
