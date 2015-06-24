using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers;
using System.Web.Security;
using VanLeeuwen.Framework.Security;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;
using System.Web;
using VanLeeuwen.Framework;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
  public class XMLDataProcessor
  {
    public XMLDataProcessor(){ }

    public Boolean ProcessDataFromXML(String databaseName, String fileContent)
    {
      Boolean returnValue = true;

      try
      {
        // Check xml type 

        // SAPECC6
        if (fileContent.Contains("IDOC"))
        {
          businessPartner customer = new businessPartner();
          DALPortalDataContext dc = new DALPortalDataContext();
					String companyCode;

          //Customer
          if (fileContent.Contains("DEBMAS03"))
          {
            if (!XMLSAPECC6Processor.ProcessCustomer(fileContent, out customer, out companyCode))
              return false;

            CreateUsers(customer, companyCode, dc);
          }
          //SalesOrder
          if (fileContent.Contains("ORDERS05"))
          {
            salesOrder document = new salesOrder();

            if (!XMLSAPECC6Processor.ProcessSalesOrder(fileContent, out document))
              return false;
          }
          //Delivery
          if (fileContent.Contains("DELVRY03"))
          {
            delivery document = new delivery();

            if (!XMLSAPECC6Processor.ProcessDelivery(fileContent, out document))
              return false;
          }

					//Item
					if (fileContent.Contains("MATMAS05"))
					{
						if (!XMLSAPECC6Processor.ProcessItem(fileContent))
							return false;
					}
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(String.Format("Process XML file Failed. Error: {0}", ex.Message), "ProcessDataFromXML");
        return false;
      }

      return returnValue;
    }

    public void CreateUsers(businessPartner customer, String companyCode, DALPortalDataContext dc)
    {
      foreach (var contactPerson in customer.contactPersons.Where(c => c.businessPartnerId.Equals(customer.businessPartnerId)))
      {
				try
				{
					if (contactPerson.eMail == null)
						continue;

					MembershipUser user = Membership.GetUser(contactPerson.eMail);

					Boolean newUser = (user == null);
					Guid newUserId;
					String password = String.Empty;

					if (newUser && contactPerson.isWebContact)
					{
						object akey = Guid.NewGuid();
						password = GeneratePassword.Generate(10, 10);

						Membership.CreateUser(contactPerson.eMail, password, contactPerson.eMail);
						Roles.AddUserToRole(contactPerson.eMail, "User");
					}

					if (contactPerson.userId == null && contactPerson.isWebContact)
					{
						newUserId = dc.Users.Where(c => c.UserName.ToLower().Equals(contactPerson.eMail.ToLower())).Select(c => c.UserId).FirstOrDefault();

						contactPerson.userId = newUserId;

						dc.contactPersons.Where(c => c.contactPersonCode.Equals(contactPerson.contactPersonCode)).FirstOrDefault().userId = newUserId;
						dc.SubmitChanges();
					}

					// Contact person in database, but not in xml file
					if (contactPerson.TMP_PortalAccess == null)
					{
						dc.contactPersons.Where(c => c.contactPersonCode.Equals(contactPerson.contactPersonCode)).FirstOrDefault().isWebContact = false;
						dc.SubmitChanges();
					}
					
					// Deactivate the user account
					// Existing user which is not a webcontact anymore or when user is not in file anymore.
					if ((!newUser && !contactPerson.isWebContact) || contactPerson.TMP_PortalAccess == null)
						if(user != null)
							if (user.IsApproved == true)
							{
								user.IsApproved = false;
								Membership.UpdateUser(user);
							}

					// Activate the user account
					if (!newUser && contactPerson.isWebContact && contactPerson.TMP_PortalAccess != null)
						if (user.IsApproved == false)
						{
							user.IsApproved = true;
							Membership.UpdateUser(user);
						}
					
					if(contactPerson.userId.HasValue)
						UpdateApplicationRoles(contactPerson, dc);

					if (newUser && contactPerson.isWebContact)
					{
						// Send Email
						SendMail(contactPerson.eMail, password, companyCode);
					}

				}
				catch (Exception ex)
				{
					Trace.WriteLine("An error occurred while creating user: " + contactPerson.eMail + ". Error: " + ex.Message, "CreateUsers");
				}
      }
    }

		private void UpdateApplicationRoles(contactPerson contactPerson, DALPortalDataContext dc)
		{
			if (!contactPerson.isWebContact)
			{
				// Delete all roles
				var userRoles = dc.userRoles.Where(c => c.userId.Equals(contactPerson.userId));
				dc.userRoles.DeleteAllOnSubmit(userRoles);

				dc.SubmitChanges();
				return;
			}

			// Add CERT_USER role
			if (contactPerson.TMP_PortalAccess == "01" || contactPerson.TMP_PortalAccess == "04")
				if (!dc.userRoles.Any(c => c.userId.Equals(contactPerson.userId) && c.roleCode.Equals("CERT_USER")))
					dc.userRoles.InsertOnSubmit(new userRole() { userId = contactPerson.userId.Value, roleCode = "CERT_USER" });

			// Add DOP_USER role
			if (contactPerson.TMP_PortalAccess == "03" || contactPerson.TMP_PortalAccess == "04")
				if (!dc.userRoles.Any(c => c.userId.Equals(contactPerson.userId) && c.roleCode.Equals("DOP_USER")))
					dc.userRoles.InsertOnSubmit(new userRole() { userId = contactPerson.userId.Value, roleCode = "DOP_USER" });

			if (contactPerson.TMP_PortalAccess == "01")
			{
				// Delete DOP_USER role
				var userRoles = dc.userRoles.Where(c => c.userId.Equals(contactPerson.userId) && c.roleCode.Equals("DOP_USER"));
				dc.userRoles.DeleteAllOnSubmit(userRoles);
			}

			if (contactPerson.TMP_PortalAccess == "03")
			{
				// Delete CERT_USER role
				var userRoles = dc.userRoles.Where(c => c.userId.Equals(contactPerson.userId) && c.roleCode.Equals("CERT_USER"));
				dc.userRoles.DeleteAllOnSubmit(userRoles);
			}

			if (contactPerson.TMP_PortalAccess == null)
			{
				// Delete all roles
				var userRoles = dc.userRoles.Where(c => c.userId.Equals(contactPerson.userId));
				dc.userRoles.DeleteAllOnSubmit(userRoles);
			}

			dc.SubmitChanges();
		}

    public void SendMail(String userName, String password, String companyCode)
    {
      String emailTemplate = Path.Combine(Parameters_DataProcessor.CompanyFilesRoot, "EmailTemplates", "MailTemplateRegisterSuccesfull_eng.html");

      StreamReader streamReader = new StreamReader(emailTemplate);
      String mailMessageBody = streamReader.ReadToEnd();

      String fromEmailAddress;
      String subject;
      String toEmailAddress;

      mailMessageBody = EmailClass.GetMailParams(mailMessageBody, out fromEmailAddress, out toEmailAddress, out subject);
      mailMessageBody = mailMessageBody.Trim();

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
        smtpClient.Host = Parameters_DataProcessor.SMTPServer;
        
				if(Parameters_DataProcessor.EmailEnabled)
					smtpClient.Send(mailMessage);
      }
      catch (Exception ex)
      {
        Trace.TraceError("An error occurred during SendMail. Message=" + ex.Message);
        Trace.WriteLine("An error occurred during SendMail. Message=" + ex.Message, "SendMail");
      }
    }
  }
}
