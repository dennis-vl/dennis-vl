using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using VanLeeuwen.Framework;
using VanLeeuwen.Framework.Security;
using VanLeeuwen.Projects.WebPortal.BusinessLogics;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Account
{
  public partial class ForgetPassword : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RadButtonSend_Click(object sender, EventArgs e)
    {
      string userName = TextBoxUserName.Text;
      MembershipUser membershipUser = Membership.GetUser(userName);

      if (membershipUser != null)
      {
        String password = GeneratePassword.Generate(10, 10);
        membershipUser.ChangePassword(membershipUser.ResetPassword(), password);

        String emailTemplate = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\EmailTemplates", "MailTemplateChangePasswordSuccesfull_eng.html"));

        StreamReader streamReader = new StreamReader(emailTemplate);
        String mailMessageBody = streamReader.ReadToEnd();

        String fromEmailAddress;
        String subject;
        String toEmailAddress;

        mailMessageBody = EmailClass.GetMailParams(mailMessageBody, out fromEmailAddress, out toEmailAddress, out subject);

        mailMessageBody = mailMessageBody.Replace("{Username}", userName);
        mailMessageBody = mailMessageBody.Replace("{Password}", password);

        try
        {
          MailMessage mailMessage = new MailMessage();
          mailMessage.IsBodyHtml = true;
          mailMessage.From = new MailAddress(fromEmailAddress);
          mailMessage.To.Add(userName);
          mailMessage.Subject = subject;
          mailMessage.Body = mailMessageBody;
          SmtpClient smtpClient = new SmtpClient();
          smtpClient.Send(mailMessage);

          PanelForgetPassword.Visible = false;
          LiteralPasswordRequestSuccesfull.Visible = true;
        }
        catch (Exception ex)
        {
          // TODO: Create errorpage for user
          Response.Redirect("~/Pages/OrderSuccess.aspx");
        }
      }
      else
      {
        LiteralUsernameNotFound.Visible = true;
      }
    }
  }
}