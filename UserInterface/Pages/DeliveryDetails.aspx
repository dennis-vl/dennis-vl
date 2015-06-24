<%@ Page Title="Certificates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryDetails.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.DeliveryDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="topPlaceHolder" runat="server">
        
          <div style="color: #fff; margin-top: -25px; width: 450px;" id="colorToChange" runat="server"> 
              <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                          <div id="padRight" style="padding-right:90px; color: #fff;  float: left;">                 
                               <asp:Literal ID="statusModel" runat="server" Text=""  />
                          </div>
                     </ContentTemplate>
                   </asp:UpdatePanel>
              <telerik:RadButton ID="updradeButton" AutoPostBack="false" runat="server" OnClientClicked="OpenWindow" Text="<%$Resources:Upgrade%>"></telerik:RadButton><telerik:RadButton ID="usageReportBut"  AutoPostBack="true" runat="server" Visible="false" OnClick="usageReportBut_Click" Text="Usage report"></telerik:RadButton>
          </div><br />
          <div style="color: #ff0000;    float: left;    font-size: 12px;    font-weight: bold;   margin-top: -20px;" id="warningDiv" runat="server"> <asp:Literal ID="warningLiteral" runat="server" Text="" Visible="false" /></div>

      <asp:Label ID="labelBpId" runat="server" Visible="true" Text=""></asp:Label>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" OnClientPageLoad="OnClientPageLoad"  Style="z-index: 7001" >

                <Windows>

                  <telerik:RadWindow

                    ID="radWindowOrderCreds" runat="server"

                    MinWidth="835px" MinHeight="650px" Modal="true" OnClientClose="OnClientClose" >

                  </telerik:RadWindow>
                    <telerik:RadWindow

                    ID="UseageReport" runat="server"

                   MinWidth="600px" MinHeight="300px" Modal="true"  >

                  </telerik:RadWindow>

                </Windows>

              </telerik:RadWindowManager>
          <asp:Label id="bpId" runat="server" Visible="true"></asp:Label>
           <script type="text/javascript" >
               function OpenWindow(sender, args) {

                   //var bpIdLabel = $("#topPlaceHolder_labelBpId").html();
                   var bpId = $("#topPlaceHolder_labelBpId").html();
                   
                   var oWnd = radopen("OrderNewCreds.aspx" + "?id=" + bpId, "radWindowOrderCreds");
                   oWnd.center();
                   return false;
               }

               function OpenReport(sender, args) {

                  // var bpIdLabel = document.getElementById("MainContent_labelBpId");
                   var bpId = $("#topPlaceHolder_labelBpId").html();

                   var oWnd = radopen("UsageReport.aspx" + "?id=" + bpId, "UseageReport");
                   oWnd.center();
                   return false;
               }

               function OnClientPageLoad(sender, args) {
                   setTimeout(function () {
                       sender.set_status("");
                   }, 0);
               }

               function OnClientClose(sender, args) {
                   location.href = document.location.toString();
               }

            </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <telerik:RadPanelBar runat="server" ID="SearchPanelBar" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="<%$Resources:Search%>" Font-Bold="true" >
                        <Items>
                            <telerik:RadPanelItem>
                                <ItemTemplate>
                                    <table ID="filterGridTable" runat="server">
                                        <tr>
                                            <td><asp:Literal ID="LiteralSalesOrder" runat="server" Text="<%$Resources:SalesOrder%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxSalesOrderNumber" EmptyMessage="<%$Resources:SalesOrder%>" DataSourceID="AutoCompleteBoxSONDataSource" DataTextField="SODocNum" InputType="Token" OnEntryAdded="RadAutoCompleteBoxSalesOrderNumber_Event" OnEntryRemoved="RadAutoCompleteBoxSalesOrderNumber_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralDelivery" runat="server" Text="<%$Resources:Delivery%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxDeliveryNumber" EmptyMessage="<%$Resources:Delivery%>" DataSourceID="AutoCompleteBoxDNDataSource" DataTextField="DELDocNum" InputType="Token" OnEntryAdded="RadAutoCompleteBoxDeliveryNumber_Event" OnEntryRemoved="RadAutoCompleteBoxDeliveryNumber_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralCustomerReference" runat="server" Text="<%$Resources:CustomerReference%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxCustomerReference" EmptyMessage="<%$Resources:CustomerReference%>" DataSourceID="AutoCompleteBoxCRDataSource" DataTextField="CustomerReference" InputType="Token" OnEntryAdded="RadAutoCompleteBoxCustomerReference_Event" OnEntryRemoved="RadAutoCompleteBoxCustomerReference_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralLineReference" runat="server" Text="<%$Resources:LineReference%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxSalesOrderLineReference" EmptyMessage="<%$Resources:LineReference%>" DataSourceID="AutoCompleteBoxSOLRDataSource" DataTextField="SOLineReference" InputType="Token" OnEntryAdded="RadAutoCompleteBoxSalesOrderLineReference_Event" OnEntryRemoved="RadAutoCompleteBoxSalesOrderLineReference_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralItem" runat="server" Text="<%$Resources:Item%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxItem" EmptyMessage="<%$Resources:Item%>" DataSourceID="AutoCompleteBoxICDataSource" DataTextField="ItemCode" InputType="Token" OnEntryAdded="RadAutoCompleteBoxItem_Event" OnEntryRemoved="RadAutoCompleteBoxItem_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
                                                    <TextSettings SelectionMode="Single" />
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="LiteralHeatNumber" runat="server" Text="<%$Resources:HeatNumber%>" /></td>
                                            <td>
                                                <telerik:RadAutoCompleteBox runat="server" ID="RadAutoCompleteBoxHeatNumber" EmptyMessage="<%$Resources:HeatNumber%>" DataSourceID="AutoCompleteBoxHNDataSource" DataTextField="HeatNumber" InputType="Token" OnEntryAdded="RadAutoCompleteBoxHeatNumber_Event" OnEntryRemoved="RadAutoCompleteBoxHeatNumber_Event" AutoPostBack="True" Width="400" DropDownWidth="400" OnClientRequesting="NoCharactersAfterOneEntryBlock">
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

            <telerik:RadGrid ID="RadGridCertificate" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="GridDataSource" GridLines="None" AllowMultiRowSelection="True" PageSize="10" AllowFilteringByColumn="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" OnItemCreated="RadGridCertificate_ItemCreated" OnItemCommand="RadGridCertificate_ItemCommand">
                <AlternatingItemStyle Height="55px" />
                <MasterTableView AutoGenerateColumns="false" DataSourceID="GridDataSource" DataKeyNames="companyCode, SODocNum,SOLineNum,DELDocId,DELDocNum,DELLineNum,DELDocDate,CustomerReference,SOLineReference,ItemCode,ItemDescription,HeatNumber,CertificateLink,batchId">
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" />

                         <telerik:GridBoundColumn DataField="batchId" HeaderText="batchId" UniqueName="batchId" ReadOnly="True" AllowFiltering="False" Display="false" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="CardCode" HeaderText="<%$Resources:CustomerCode%>" UniqueName="CardCode" ReadOnly="True" AllowFiltering="False" Visible="false" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="bpName" HeaderText="Company name" UniqueName="bpName" ReadOnly="True" AllowFiltering="False" Visible="false" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="SODocNum" HeaderText="<%$Resources:SalesOrder%>" UniqueName="SODocNum" ReadOnly="True" AllowFiltering="False">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SOLineNum" HeaderText="<%$Resources:SOLine%>" UniqueName="SOLineNum" ReadOnly="True" ShowFilterIcon="False" CurrentFilterFunction="StartsWith" AutoPostBackOnFilter="true" FilterControlWidth="20px">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DELDocNum" HeaderText="<%$Resources:Delivery%>" UniqueName="DELDocNum" ReadOnly="True" AllowFiltering="False">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DELLineNum" HeaderText="<%$Resources:DelLine%>" UniqueName="DELLineNum" ReadOnly="True" ShowFilterIcon="False" CurrentFilterFunction="StartsWith" AutoPostBackOnFilter="true" FilterControlWidth="20px">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <%--<telerik:GridDateTimeColumn DataField="DELDocDate" HeaderText="<%$Resources:DelDocDate%>" UniqueName="DELDocDate" ReadOnly="True" AllowFiltering="False" DataFormatString="{0:d}">--%>
                        <telerik:GridDateTimeColumn DataField="DELDocDate" HeaderText="<%$Resources:DelDocDate%>" UniqueName="DELDocDate" ReadOnly="True" AllowFiltering="False" DataFormatString="{0:dd-MM-yyyy}">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn DataField="CustomerReference" HeaderText="<%$Resources:CustomerReference%>" UniqueName="CustomerReference" ReadOnly="True" AllowFiltering="False">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SOLineReference" HeaderText="<%$Resources:LineReference%>" UniqueName="SOLineReference" ReadOnly="True" AllowFiltering="False">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode" HeaderText="<%$Resources:ItemCode%>" UniqueName="ItemCode" ReadOnly="True" AllowFiltering="False" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemDescription" HeaderText="<%$Resources:ItemDescription%>" UniqueName="ItemDescription" ReadOnly="True" AllowFiltering="False" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn  DataField="HeatNumber" HeaderText="<%$Resources:HeatNumber%>" UniqueName="HeatNumber" ReadOnly="True" AllowFiltering="False"  >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CertificateLink" UniqueName="CertificateLink" ReadOnly="True" AllowFiltering="False" Display="false" >
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>                   
                        <telerik:GridTemplateColumn  ReadOnly="True" HeaderText="Download" UniqueName="DownloadCertificate" AllowFiltering="False" >
                            <ItemTemplate>
                                <asp:LinkButton   ID="CertificateHyperLink" runat="server"  CommandName="Outsource" Visible='<%# String.IsNullOrEmpty(Convert.ToString(Eval("CertificateLink"))) ? false : true %>'>
                                    <img src="../Images/Adobe_PDF_Icon_svg.png" height="43"  alt="Show Certificate" border="0" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
     
            <div id="hiddenStatusText" style="display: none;">                 
                  <asp:Literal ID="satusModelLit" runat="server" Text=""  />
            </div>
            </ContentTemplate>
    </asp:UpdatePanel>

    <div id="gridButtonsDiv">
        <telerik:RadButton ID="RadButtonMergePDFs" runat="server" Text="Print" OnClick="RadButtonMergePDFs_Click" Width="75px" />
        <telerik:RadButton ID="RadButtonSelectedInZip" runat="server" Text="Download"  OnClick="RadButtonSelectedInZip_Click"  Width="75px" />
        <div>
            <asp:Literal ID="LiteralNoCertificatesSelected" runat="server" Text="<%$Resources:NoCertificatesSelected%>" Visible="false" />           
        </div>
    </div>
    <iframe id='my_iframe' style='display:none;'>
    </iframe>
    <script type="text/javascript">
       // changeStatusModel();
        function changeStatusModel()
        {
            jQuery("#MainContent_statusModel").html("Test stuff");
        }
        setContentTop();
        function setContentTop()
        {
            $("#topPlaceHolder_labelBpId").hide();
            if ($("#topPlaceHolder_warningDiv").html().length > 1)
            {
                $('#content').css('margin-top', 30);
            }else
            {
                $('#content').css('margin-top', 10);
            }
        }
        setInTopPlaceHolder();
        function setInTopPlaceHolder()
        {
            // $("#topPlaceHolder_colorToChange")
            $("#loginBox").append($("#topPlaceHolder_colorToChange"));
            $("#loginBox").append($("#topPlaceHolder_warningDiv"));
        }

        function updatePanel2Update(sender, args) {
          //  __doPostBack("<%=UpdatePanel1.UniqueID %>", "");
        }

        function setContentHeader()
        {
          var innerHtml = $("#hiddenStatusText").html();
          $("#padRight").html(innerHtml);
        }

        function Download(url) {
            //var url = "http://localhost:57474/Files/Certificates/3331224-3-201514.46.15.zip";
            document.getElementById('my_iframe').src = url;
        };

        
    </script>
        
  

    <asp:LinqDataSource ID="AutoCompleteBoxSONDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (SODocNum)"  OnSelecting="AutoCompleteBoxSONDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxDNDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (DELDocNum)" OnSelecting="AutoCompleteBoxDNDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxCRDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (CustomerReference)" OnSelecting="AutoCompleteBoxCRDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxSOLRDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (SOLineReference)" OnSelecting="AutoCompleteBoxSOLRDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxICDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (ItemCode)" OnSelecting="AutoCompleteBoxIDataSource_Selecting" />
    <asp:LinqDataSource ID="AutoCompleteBoxHNDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (HeatNumber)" OnSelecting="AutoCompleteBoxHNDataSource_Selecting" />

    <asp:LinqDataSource ID="AutoCompleteBoxCardCodeDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" TableName="vw_DeliveryLines" Select="new (CardCode)" OnSelecting="AutoCompleteBoxCardCodeDataSource_Selecting" />

    <asp:LinqDataSource ID="GridDataSource" runat="server" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" OnSelecting="GridDataSource_Selecting" />

</asp:Content>
