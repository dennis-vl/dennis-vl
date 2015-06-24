<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAccountDetails.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Pages.CustomerAccountDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
          <fieldset>           
                <legend>
                    <asp:Literal ID="Literal1" runat="server" Text="Customer details" />
                </legend>
                <table id="userTable">
                <tr>
                    <td style="width: 220px;">
                        <asp:Literal ID="companyCode" text="<%$Resources:CompanyCode%>" runat="server"/>
                    </td>
                    <td>
                        <asp:Literal ID="companyCode2" text="" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 220px;">
                        <asp:Literal ID="company" text="<%$Resources:Company%>" runat="server"/>
                    </td>
                    <td>
                        <asp:Literal ID="company2" text="" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelCountry" runat="server" Text="<%$Resources:Country%>" />
                    </td>
                    <td>
                        <asp:Label ID="LabelCountry2" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:Label ID="AccountManager" runat="server" Text="Account manager" />
                </td>
                <td>
                    <telerik:RadDropDownList ID="accountManagerDDL" DataValueField="UserId" DataTextField="name" runat="server" DataSourceID="LinqDataSource1"></telerik:RadDropDownList>
                    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext"  TableName="Users" OnSelecting="LinqDataSource1_Selecting"></asp:LinqDataSource>
                </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelMotherCompany" runat="server" Text="<%$Resources:MotherCompany%>" /> <asp:CheckBox ID="checkBoxMother" AutoPostBack="true" runat="server" />
                    </td>
                    <td>
                        <telerik:RadButton ID="selectSistersButton" runat="server" Text="<%$Resources:SelectSisters%>" OnClick="selectSistersButton_Click"></telerik:RadButton>
                    </td>
                </tr>
            
              </table>
          </fieldset>
          <fieldset>           
                <legend>
                    <asp:Literal ID="Literal2" runat="server" Text="Portal Pages" />
                </legend>
                <table id="portalTable">
                <tr>
                    <td style="width: 220px;">
                        <asp:Literal ID="dopLiteral" text="DOP: " runat="server"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="dopCheckBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 220px;">
                        <asp:Literal ID="Literal5" text="Certificates" runat="server"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="certificatesCheckBox" runat="server" />
                        <asp:ImageButton ID="ImageButton1" OnClick="ImageButton1_Click" ImageUrl="~/Images/magnifier.png" runat="server" />
                    </td>
                </tr>  
                <tr>
                    <td style="width: 220px;">
                        <telerik:RadButton ID="saveButton" runat="server" Text="<%$Resources:Save%>" OnClick="saveButton_Click"></telerik:RadButton>
                        <telerik:RadButton ID="cancelButton" runat="server" Text="<%$Resources:Cancel%>" OnClick="cancelButton_Click"></telerik:RadButton>
                    </td>
                </tr> 
                       
              </table>
          </fieldset>
        <fieldset>           
                <legend>
                    <asp:Literal ID="Literal3" runat="server" Text="Contactpersons" />
                </legend>
        
                        <telerik:RadGrid ID="contactPersonGrid"  runat="server" CellSpacing="-1" DataSourceID="contactPersonDataSource"  GridLines="Both" GroupPanelPosition="Top" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowMultiRowSelection="True" ItemStyle-Height="55px" AlternatingItemStyle-Height="55px" Culture="<%# System.Globalization.CultureInfo.CurrentCulture %>" >
                            <MasterTableView DataSourceID="contactPersonDataSource" AutoGenerateColumns="False">
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="nameColumn" HeaderText="Name" >                          
                                         <ItemTemplate>
                                              <%# Eval("firstName") %>  <%# Eval("lastName") %>
                                          </ItemTemplate>
                                          <EditItemTemplate>
                                              <table>
                                                 <tr>
                                                   <td style="width: 50%">
                                                     <asp:TextBox ID="firstNameTxt" runat="server" Text='<%# Bind("firstName") %>' />
                                                   </td>
                                                   <td style="width: 50%">
                                                      <asp:TextBox ID="lastNameTxt" runat="server" Text='<%# Bind("lastName") %>' />
                                                   </td>
                                                  </tr>
                                               </table>
                                          </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="lastName" ReadOnly="True" HeaderText="<%$Resources:LastName%>" SortExpression="lastName" UniqueName="lastName" FilterControlAltText="Filter lastName column">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="eMail" ReadOnly="True" HeaderText="User name" SortExpression="eMail" UniqueName="eMail" FilterControlAltText="Filter eMail column">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
        </fieldset>
            <asp:LinqDataSource runat="server" EntityTypeName="" ID="contactPersonDataSource" ContextTypeName="VanLeeuwen.Projects.WebPortal.DataAccess.Database.DALPortalDataContext" Select="new (firstName, lastName, eMail)" TableName="contactPersons" Where="businessPartnerId == @businessPartnerId">
                <WhereParameters>
                    <asp:SessionParameter SessionField="businessPartnerId" Name="businessPartnerId" Type="Int32"></asp:SessionParameter>
                </WhereParameters>
            </asp:LinqDataSource>  
    

        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
