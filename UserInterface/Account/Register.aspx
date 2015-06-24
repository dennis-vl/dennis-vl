<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="VanLeeuwen.Projects.WebPortal.UserInterface.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <script type="text/javascript">
        function AlphabetOnly(sender, eventArgs) {
            var c = eventArgs.get_keyCode();
            if ((c < 65) || (c > 90 && c < 97) || (c > 122))
                eventArgs.set_cancel(true);
        }

        function BlurTest(sender, eventArgs) {
            //var rfv = sender.get_id();
            window.alert(sender.get_id());
            //Page_ClientValidate();
            //var rfv = document.getElementById("<%= RequiredFieldValidatorEmail.ClientID %>");
            //ValidatorValidate(rfv);
        }

        function OnBlurCompanyName() {
            var rfv = document.getElementById("<%= RequiredFieldValidatorCompanyName.ClientID %>");
            ValidatorValidate(rfv);
        }
        function OnBlurInitials() {
            var rfv = document.getElementById("<%= RequiredFieldValidatorInitials.ClientID %>");
            ValidatorValidate(rfv);
        }
        function OnBlurLastName() {
            var rfv = document.getElementById("<%= RequiredFieldValidatorLastName.ClientID %>");
            ValidatorValidate(rfv);
        }
        function OnBlurEmail() {
            var rfv = document.getElementById("<%= RequiredFieldValidatorEmail.ClientID %>");
            ValidatorValidate(rfv);
        }
        function OnBlurPhoneNumber() {
            var rfv = document.getElementById("<%= RequiredFieldValidatorPhoneNumber.ClientID %>");
            ValidatorValidate(rfv);
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanelRegistration" runat="server">
        <ContentTemplate>

            <asp:Panel ID="PanelRegistrationSuccesfull" runat="server" Visible="False">
                <asp:Literal ID="LiteralRequestSuccesfull" runat="server" Text="<%$Resources:RequestSuccesfull%>" />
            </asp:Panel>
            <asp:Panel ID="PanelRegistration" runat="server">
                <div id="registrationIntroText">
                    <asp:Literal ID="LiteralFillFormAndReceiveDetailsByMail" runat="server" Text="<%$Resources:FillFormAndReceiveDetailsByMail%>" />
                </div>

                <fieldset>
                    <legend><asp:Literal ID="LiteralRegistrationForm" runat="server" Text="<%$Resources:RegistrationForm%>" /></legend>
                    <table id="registrationTable">
                        <tr>
                            <td>
                                <asp:Literal ID="LiteralCompanyName" runat="server" Text="<%$Resources:CompanyName%>" />
                                <span class="requiredRegistrationFieldMarker"> *</span>
                            </td>
                            <td>
                                <asp:Literal ID="LiteralAddress" runat="server" Text="<%$Resources:Address%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxCompanyName" runat="server" EmptyMessage="<%$Resources:CompanyName%>">
                                    <ClientEvents OnBlur="OnBlurCompanyName" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyName" runat="server" ControlToValidate="RadTextBoxCompanyName" ErrorMessage="<%$Resources:CompanyNameIsRequired%>" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxAddress" runat="server" EmptyMessage="<%$Resources:Address%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="LiteralInitials" runat="server" Text="<%$Resources:Initials%>" />
                                <span class="requiredRegistrationFieldMarker"> *</span>
                            </td>
                            <td>
                                <asp:Literal ID="LiteralZipCode" runat="server" Text="<%$Resources:ZipCode%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxInitials" runat="server" EmptyMessage="<%$Resources:Initials%>">
                                    <ClientEvents OnBlur="OnBlurInitials" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInitials" runat="server" ControlToValidate="RadTextBoxInitials" ErrorMessage="<%$Resources:InitialsAreRequired%>" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxZipCode" runat="server" EmptyMessage="<%$Resources:ZipCode%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="LiteralLastName" runat="server" Text="<%$Resources:LastName%>" />
                                <span class="requiredRegistrationFieldMarker"> *</span>
                            </td>
                            <td>
                                <asp:Literal ID="LiteralCountry" runat="server" Text="<%$Resources:Country%>" />
                                <span class="requiredRegistrationFieldMarker"> *</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxLastName" runat="server" EmptyMessage="<%$Resources:LastName%>">
                                    <ClientEvents OnKeyPress="AlphabetOnly" OnBlur="OnBlurLastName" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorLastName" runat="server" ControlToValidate="RadTextBoxLastName" ErrorMessage="<%$Resources:LastNameIsRequired%>" />
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="RadDropDownListCountry" runat="server" DefaultMessage="<%$Resources:Country%>">
                                    <Items>
                                        <telerik:DropDownListItem Text="Netherlands" Value="Netherlands" />
                                        <telerik:DropDownListItem Text="Belgium" Value="Belgium" />
                                        <telerik:DropDownListItem Text="Poland" Value="Poland" />
                                        <telerik:DropDownListItem Text="Czech Republic" Value="Czech Republic" />
                                    </Items>
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="RadDropDownListCountry" ErrorMessage="<%$Resources:CountryIsRequired%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="LiteralTelephone" runat="server" Text="<%$Resources:Telephone%>" />
                                <span class="requiredRegistrationFieldMarker"> *</span>
                            </td>
                            <td>
                                <asp:Literal ID="LiteralCity" runat="server" Text="<%$Resources:City%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxPhoneNumber" runat="server" EmptyMessage="<%$Resources:Telephone%>">
                                    <ClientEvents OnBlur="OnBlurPhoneNumber" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPhoneNumber" runat="server" ControlToValidate="RadTextBoxPhoneNumber" ErrorMessage="<%$Resources:PhoneNumberIsRequired%>" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxCity" runat="server" EmptyMessage="<%$Resources:City%>">
                                    <ClientEvents OnKeyPress="AlphabetOnly" />
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                E-mail <span class="requiredRegistrationFieldMarker">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="RadTextBoxEmail" runat="server" EmptyMessage="E-mail">
                                    <ClientEvents OnBlur="OnBlurEmail" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ControlToValidate="RadTextBoxEmail" ErrorMessage="<%$Resources:EMailIsRequired%>" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="<%$Resources:EMailNotValid%>" ControlToValidate="RadTextBoxEmail" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadCaptcha ID="RadCaptchaRegistration" runat="server" CaptchaImage-BackgroundNoise="Extreme" CaptchaTextBoxLabel="<%$Resources:EnterCode%>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="RadButtonSend" runat="server" Text="<%$Resources:Send%>" OnClick="RadButtonSend_Click" />
                                <asp:HyperLink ID="HyperLinkCancel" runat="server" NavigateUrl="~/Account/Login.aspx" Text="<%$Resources:Cancel%>" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
