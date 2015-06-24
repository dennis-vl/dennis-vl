<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnAuthorised.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.UnAuthorised" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="LiteralSalesOrder" runat="server" Text="<%$Resources:GlobalResource, NotAuthorised %>" />
</asp:Content>
