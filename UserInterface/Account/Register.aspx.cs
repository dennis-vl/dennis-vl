using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Net.Mail;
using System.IO;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Framework;
using VanLeeuwen.Projects.WebPortal.BusinessLogics;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Account
{
  public partial class Register : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void RadButtonSend_Click(object sender, EventArgs e)
    {
      if (Page.IsValid)
      {
        String companyName = RadTextBoxCompanyName.Text;
        String address = RadTextBoxAddress.Text;

        String initials = RadTextBoxInitials.Text;
        String zipCode = RadTextBoxZipCode.Text;

        String lastName = RadTextBoxLastName.Text;
        String country = RadDropDownListCountry.SelectedValue;

        String phoneNumber = RadTextBoxPhoneNumber.Text;
        String city = RadTextBoxCity.Text;
        
        String email = RadTextBoxEmail.Text;

        StreamReader StreamReader = new StreamReader(HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\EmailTemplates\RegisterAccountMailTemplate.html")));
        String mailMessageBody = StreamReader.ReadToEnd();

        String fromEmailAddress;
        String toEmailAddress;
        String subject;

        mailMessageBody = EmailClass.GetMailParams(mailMessageBody, out fromEmailAddress, out toEmailAddress, out subject);

        mailMessageBody = mailMessageBody.Replace("companyNameValue", companyName);
        mailMessageBody = mailMessageBody.Replace("addressValue", address);

        mailMessageBody = mailMessageBody.Replace("initialsValue", initials);
        mailMessageBody = mailMessageBody.Replace("zipCodeValue", zipCode);

        mailMessageBody = mailMessageBody.Replace("lastNameValue", lastName);
        mailMessageBody = mailMessageBody.Replace("countryValue", country);

        mailMessageBody = mailMessageBody.Replace("phoneNumberValue", phoneNumber);
        mailMessageBody = mailMessageBody.Replace("cityValue", city);
        
        mailMessageBody = mailMessageBody.Replace("emailValue", email);

        //
        try
        {
          MailMessage mailMessage = new MailMessage();
          mailMessage.IsBodyHtml = true;
          mailMessage.From = new MailAddress(fromEmailAddress);
          mailMessage.To.Add(toEmailAddress);
          mailMessage.Subject = subject;
          mailMessage.Body = mailMessageBody;
          SmtpClient smtpClient = new SmtpClient();
          smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
          Response.Write("Could not send the e-mail - error: " + ex.Message);
        }

        PanelRegistration.Visible = false;
        PanelRegistrationSuccesfull.Visible = true;
      }
    }
  }
}
