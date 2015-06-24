using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Framework;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics
{
  public class EmailClass
  {
    public static string GetMailParams(String mailMessageBody, out string fromEmailAddress, out string toEmailAddress, out string subject)
    {
      fromEmailAddress = StringFunctions.GetPartOfString(StringFunctions.GetPartOfString(mailMessageBody, "<fromEmailAddress>", 2), "</fromEmailAddress>", 1);
      toEmailAddress = StringFunctions.GetPartOfString(StringFunctions.GetPartOfString(mailMessageBody, "<toEmailAddress>", 2), "</toEmailAddress>", 1);
      subject = StringFunctions.GetPartOfString(StringFunctions.GetPartOfString(mailMessageBody, "<subject>", 2), "</subject>", 1);

      mailMessageBody = mailMessageBody.Replace("<fromEmailAddress>", "");
      mailMessageBody = mailMessageBody.Replace("</fromEmailAddress>", "");
      if (!String.IsNullOrEmpty(fromEmailAddress))
        mailMessageBody = mailMessageBody.Replace(fromEmailAddress, "");

      mailMessageBody = mailMessageBody.Replace("<toEmailAddress>", "");
      mailMessageBody = mailMessageBody.Replace("</toEmailAddress>", "");
      if(!String.IsNullOrEmpty(toEmailAddress))
        mailMessageBody = mailMessageBody.Replace(toEmailAddress, "");

      mailMessageBody = mailMessageBody.Replace("<subject>", "");
      mailMessageBody = mailMessageBody.Replace("</subject>", "");
      if (!String.IsNullOrEmpty(subject))
        mailMessageBody = mailMessageBody.Replace(subject, "");

      mailMessageBody = mailMessageBody.Trim();
      return mailMessageBody;
    }
  }
}
