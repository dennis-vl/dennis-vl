<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsageReport.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.UseageReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form runat="server">
        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/Adobe_PDF_Icon_svg.png" Width="50px"  OnClick="PrintOnScreen_Click" AlternateText="Biff" />     <asp:ImageButton ID="ImageButton4"  Width="50px" runat="server" ImageUrl="~/Images/Excel_XLSX.png"   OnClick="ExportExcel_Click" AlternateText="Xlsx" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
                <telerik:RadGrid ID="RadGrid1" DataSourceID="uReport" runat="server" GroupPanelPosition="Top" CellSpacing="-1" GridLines="Both" OnItemCreated="RadGrid1_ItemCreated" >
                    <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                            BorderStyle="Medium" BorderColor="#666666">
                        </Pdf>
                    </ExportSettings>
                    <MasterTableView DataSourceID="uReport" AutoGenerateColumns="False">
                        <Columns>
                            <telerik:GridBoundColumn DataField="savedMark" ReadOnly="True" HeaderText="<%$Resources:DateTime%>" SortExpression="savedMark" UniqueName="savedMark" DataType="System.DateTime" FilterControlAltText="Filter savedMark column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="contactPerson.lastName" ReadOnly="True" HeaderText="<%$Resources:ContactPerson%>" SortExpression="contactPerson.lastName" UniqueName="contactPerson.lastName" FilterControlAltText="Filter contact person column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="contactPerson.businessPartner.bpName" ReadOnly="True" HeaderText="<%$Resources:BusinessPartner%>" SortExpression="contactPerson.lastName" UniqueName="contactPerson.lastName" FilterControlAltText="Filter contact person column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="delivery.documentNumber" ReadOnly="True" HeaderText="<%$Resources:DeliveryDocument%>" SortExpression="documentNumber" UniqueName="documentNumber" FilterControlAltText="Filter documentNumber column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="baseLineNum" ReadOnly="True" HeaderText="<%$Resources:DeliveryLine%>" SortExpression="baseLineNum" UniqueName="baseLineNum" DataType="System.Int32" FilterControlAltText="Filter baseLineNum column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="heatNumber" ReadOnly="True" HeaderText="<%$Resources:HeatNumber%>" SortExpression="heatNumber" UniqueName="heatNumber" FilterControlAltText="Filter heatNumber column">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>

                <asp:LinqDataSource ID="uReport" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" EntityTypeName="" Select="new (savedMark, heatNumber, contactPerson, baseDocId, baseLineNum, delivery)" TableName="usageReports" Where="businessPartnerId == @businessPartnerId">
                    <WhereParameters>
                        <asp:QueryStringParameter QueryStringField="id" Name="businessPartnerId" Type="Int32"></asp:QueryStringParameter>
                    </WhereParameters>
                </asp:LinqDataSource>
                
        </form>
</body>
</html>
