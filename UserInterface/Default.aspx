<%@ Page Title="Welcome" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface._Default" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    Welkom
    <div class="textDivWithTopBorder">
        <asp:LoginName ID="LoginName1" runat="server" /> welkom  op de Van Leeuwen klantenportaal. 
        Klik <asp:HyperLink ID="HyperLinkCertificate" runat="server" NavigateUrl="~/Pages/DeliveryDetails.aspx">hier</asp:HyperLink> om naar het het certificatenoverzicht te gaan.
    </div>
</asp:Content>
