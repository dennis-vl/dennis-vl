<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderNewCreds.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.OrderNewCreds" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/Site.css" />
    <title>Upgrade certificate credit</title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>    
            <div>  
                    <table id="userTable">
                        <tr>
                            <td style="width: 420px;">
                                <asp:Literal ID="creditsPurchacedLiteral" text="<%$Resources:CreditsPurchased%>" runat="server"/>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="creditsPurchasedTxtBox" Enabled="false"  NumberFormat-DecimalDigits="0" runat="server"></telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 420px;">
                                <asp:Literal ID="Literal2" text="<%$Resources:CreditsUsed%>" runat="server"/>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="creditsUsedTxtBox" Enabled="false"  NumberFormat-DecimalDigits="0" runat="server"></telerik:RadNumericTextBox><telerik:RadButton ID="usageReport" AutoPostBack="true" runat="server" OnClick="usageReport_Click" Text="Usage report"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>

                        </tr>
                        <tr>
                            <td style="width: 420px;">
                                <asp:Literal ID="Literal3" text="<%$Resources:NewCredits%>" runat="server"/>
                            </td>
                            <td>
                                <telerik:RadButton ID="minButton" OnClick="minButton_Click" runat="server" Text="-"></telerik:RadButton><telerik:RadNumericTextBox ID="creditsOrderTxtBox" Enabled="false"  NumberFormat-DecimalDigits="0" runat="server" Value="50"  Width="108px"></telerik:RadNumericTextBox><telerik:RadButton ID="plusButton" runat="server" Text="+" OnClick="plusButton_Click"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 420px;">
                                <telerik:RadButton ID="RadSaveButton" runat="server" Text="<%$Resources:Upgrade%>" OnClick="RadSaveButton_Click" ></telerik:RadButton> <telerik:RadButton ID="RadButton1" runat="server" Text="<%$Resources:Cancel%>" OnClientClicking="Close"  ></telerik:RadButton> 
                            </td>
                        </tr>
                        <asp:Label ID="labelBpId" runat="server" Visible="true" CssClass="hiddencol" Text=""></asp:Label>
                    </table>
                </div>
            </ContentTemplate>
	    </asp:UpdatePanel>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"  >

                <Windows>

                  <telerik:RadWindow

                    ID="UseageReport" runat="server"

                    MinWidth="600px" MinHeight="300px" Modal="true"  >

                  </telerik:RadWindow>

                </Windows>

              </telerik:RadWindowManager>
            <script type="text/javascript" >

                function GetRadWindow() {
                    var oWnd = null;
                    if (window.radWindow) oWnd = window.radWindow;
                    else if (window.frameElement.radWindow) oWnd = window.frameElement.radWindow;
                    return oWnd;
                }

                function OpenReport(sender, args) {

                    var bpIdLabel = document.getElementById("labelBpId");
                    var bpId = bpIdLabel.innerHTML;

                    var oWnd = radopen("UsageReport.aspx" + "?id=" + bpId, "UseageReport");
                    oWnd.center();
                    return false;
                }

                function Close(sender, args) {

                    GetRadWindow().close();
                }


     </script>
    </form>

</body>
</html>
