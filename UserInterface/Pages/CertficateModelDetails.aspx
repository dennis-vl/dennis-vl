<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CertficateModelDetails.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.CertficateModelDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <fieldset>           
            <legend>
                <asp:Literal ID="Literal1" runat="server" Text="Certificate details" />
            </legend>
            <table id="userTable">
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="modelLiteral1" text="<%$Resources:ModelOne%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadButton ID="btnModel1" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton" >
                        <ToggleStates>
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadioChecked" />
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadio" />
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="modelLiteral2" text="<%$Resources:ModelTwo%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadButton ID="btnModel2" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
                         <ToggleStates>
                            <telerik:RadButtonToggleState Value="selected"  PrimaryIconCssClass="rbToggleRadioChecked" />
                            <telerik:RadButtonToggleState Value="notSelected"  PrimaryIconCssClass="rbToggleRadio" />
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="modelLiteral3" text="<%$Resources:ModelTree%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadButton ID="btnModel3" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
                         <ToggleStates>
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadioChecked" />
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadio" />
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>      
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="modelLiteral4" text="<%$Resources:ModelFour%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadButton ID="btnModel4" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
                         <ToggleStates>
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadioChecked" />
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadio" />
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>  
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="modelLiteral5" text="<%$Resources:ModelFive%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadButton ID="btnModel5" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
                         <ToggleStates>
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadioChecked" />
                            <telerik:RadButtonToggleState  PrimaryIconCssClass="rbToggleRadio" />
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>  
            </table> 
      
        </fieldset>
            <table id="userTable2">
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="creditsPurchacedLiteral" text="<%$Resources:CreditsPurchased%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="creditsPurchasedTxtBox"  Enabled="false"  Value="0" NumberFormat-DecimalDigits="0" runat="server"></telerik:RadNumericTextBox><telerik:RadButton ID="updradeButton" AutoPostBack="false" runat="server" OnClientClicked="OpenWindow" Text="Order new credits"></telerik:RadButton> 
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="creditsUsedLiteral" text="<%$Resources:CreditsUsed%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="creditsUsedTxtBox" Enabled="false" Value="0" NumberFormat-DecimalDigits="0" runat="server"></telerik:RadNumericTextBox><telerik:RadButton ID="usageReport" AutoPostBack="true" runat="server" OnClick="usageReport_Click" Text="Usage report"></telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="creditsLeftLiteral" text="<%$Resources:CreditsLeft%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="creditsLeftTxtBox" NumberFormat-DecimalDigits="0"  Value="0"  ShowSpinButtons="true" runat="server"></telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="validUntilLiteral" text="<%$Resources:ValidUntil%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadDateTimePicker ID="validUntilDTP" TimeView-Enabled="false"  DateInput-DateFormat="dd/MM/yyyy" TimePopupButton-Enabled="false" TimePopupButton-Visible="false" runat="server"></telerik:RadDateTimePicker>
                </td>
            </tr>
            <tr>
                <td style="width: 420px;">
                    <asp:Literal ID="Literal2" text="<%$Resources:CertificatePrice%>" runat="server"/>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="priceTextBox" Type="Currency" Value="7.50" NumberFormat-DecimalDigits="2" runat="server"></telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="RadSaveButton" runat="server" Text="<%$Resources:Save%>" OnClick="RadSaveButton_Click"></telerik:RadButton><telerik:RadButton ID="RadCancelButton" runat="server" Text="Cancel" OnClick="RadCancelButton_Click"></telerik:RadButton>
                </td>
            </tr>        
      </table>
      <asp:Label ID="labelBpId" runat="server" Visible="true" CssClass="hiddencol" Text=""></asp:Label>

              </ContentTemplate>
        </asp:UpdatePanel>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server"  OnClientPageLoad="OnClientPageLoad"  Style="z-index: 7001" >

                <Windows>

                  <telerik:RadWindow

                    ID="radWindowOrderCreds" runat="server"

                    MinWidth="835px" MinHeight="350px" Modal="true" OnClientClose="OnClientClose" >

                  </telerik:RadWindow>
                     <telerik:RadWindow

                    ID="UseageReport" runat="server"

                   MinWidth="600px" MinHeight="300px" Modal="true"  >

                  </telerik:RadWindow>

  

                </Windows>

              </telerik:RadWindowManager>
          <asp:Label id="bpId" runat="server" Visible="false"></asp:Label>
           <script type="text/javascript" >
               function OpenWindow(sender, args) {
                   
                   var bpIdLabel = document.getElementById("MainContent_labelBpId");
                   var bpId = bpIdLabel.innerHTML;

                   var oWnd = radopen("OrderNewCreds.aspx" + "?id=" + bpId , "radWindowOrderCreds");
                   oWnd.center();
                   return false;
               }

               function OpenReport(sender, args) {

                   var bpIdLabel = document.getElementById("MainContent_labelBpId");
                   var bpId = bpIdLabel.innerHTML;

                   var oWnd = radopen("UsageReport.aspx" + "?id=" + bpId, "UseageReport");
                   oWnd.center();
                   return false;
               }

               function OnClientPageLoad(sender, args) {
                   setTimeout(function () {
                       sender.set_status("");
                   }, 0);
               }

               function OnClientClose(sender, args)
               {
                   location.href = document.location.toString();
               }

            </script>
</asp:Content>
