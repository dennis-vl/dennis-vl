<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderSuccess.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.OrderSuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
          <asp:Literal ID="Literal3" text="Order successful. Confirmation has been send." runat="server"/> <br />
            <telerik:RadButton ID="RadButton1" runat="server" Text="Close" OnClientClicking="Close"  ></telerik:RadButton>
    </div>
         <script type="text/javascript" >

             function GetRadWindow() {
                 var oWnd = null;
                 if (window.radWindow) oWnd = window.radWindow;
                 else if (window.frameElement.radWindow) oWnd = window.frameElement.radWindow;
                 return oWnd;
             }

             function Close(sender, args) {

                 GetRadWindow().close();
             }


     </script>
    </form>
</body>
</html>
