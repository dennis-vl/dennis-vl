using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using VanLeeuwen.Projects.WebPortal.DataAccess.CachedObjects;
using VanLeeuwen.Framework.Xml;
using System.Configuration;
using System.IO;
using System.Web.Security;
using VanLeeuwen.Projects.WebPortal.BusinessLogics;
using System.Net.Mail;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class OrderNewCreds : BasePage
	{
		private static int bpId;
		protected void Page_Load(object sender, EventArgs e)
		{
			string id = Request.QueryString["id"];
			labelBpId.Text = id;
			bpId = Convert.ToInt32(id);

			loadData();
		}

		private void loadData()
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			certificateBundle bundleOud = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(bpId) && c.isActive.Equals(true)).SingleOrDefault();
			if (bundleOud != null)
			{
				creditsPurchasedTxtBox.Value = bundleOud.orderedCertQty;
				if (bundleOud.usedCertQty != null)
				{
					creditsUsedTxtBox.Value = bundleOud.usedCertQty;
				}
			}
		}

		protected void RadSaveButton_Click(object sender, EventArgs e)
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			certificateBundle bundleOld = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(bpId) && c.isActive.Equals(true)).SingleOrDefault();

			if (bundleOld != null)
			{
				bundleOld.isActive = false;
			}
			certificateBundle bundelNew = new certificateBundle();
			if (bundleOld != null)
			{
				bundelNew.previousCertQty = bundleOld.actualCertQty;
				bundelNew.actualCertQty = (Int32)creditsOrderTxtBox.Value + bundleOld.actualCertQty;
				bundelNew.orderedCertQty = bundleOld.orderedCertQty + (Int32)creditsOrderTxtBox.Value;
				bundelNew.certificatePrice = bundleOld.certificatePrice;
			}
			else
			{
				bundelNew.previousCertQty = 0;
				bundelNew.actualCertQty = (Int32)creditsOrderTxtBox.Value;
				bundelNew.orderedCertQty = (Int32)creditsOrderTxtBox.Value;
				bundelNew.certificatePrice = (decimal)7.50;
			}

			bundelNew.startDate = DateTime.Now;
			bundelNew.expireDate = DateTime.Now.AddYears(1);

			bundelNew.isActive = true;
			bundelNew.modelId = 2;
			bundelNew.companyCode = "ZW";
			bundelNew.businessPartnerId = bpId;

			businessPartner bpProps = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).SingleOrDefault();


			DocumentCached cashed = new DocumentCached();
			cashed.CustomerCode = bpProps.bpCode;
			cashed.VLCompany = "ZW01";
			cashed.DocDate = DateTime.Now;
			cashed.Comment = "Certificate portal invoice";

			DocumentLineCached cashedLine = new DocumentLineCached();

			cashedLine.Currency = "EUR";
			cashedLine.LineNum = 1;
			cashedLine.ItemCode = "C";
			cashedLine.Quantity = (Int32)creditsOrderTxtBox.Value;
			cashedLine.UnitOfMeasure = "ST";
			cashedLine.Price = (decimal)bundelNew.certificatePrice;
			cashedLine.ShortText = creditsOrderTxtBox.Value + " ordered on " + DateTime.Now + ".";
			cashed.Lines.Add(cashedLine);
			dc.certificateBundles.InsertOnSubmit(bundelNew);

			dc.SubmitChanges();

			XmlToObject.ObjectToXml(cashed, HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Outbox", "invoice " + DateTime.Now.Ticks.ToString() + ".xml")));


      MembershipUser membershipUser = Membership.GetUser();

      if (membershipUser != null)
      {
        String emailTemplate = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\EmailTemplates", "OrderConfirmation.html"));

        StreamReader streamReader = new StreamReader(emailTemplate);
        String mailMessageBody = streamReader.ReadToEnd();

        String fromEmailAddress;
        String subject;
        String toEmailAddress;

        mailMessageBody = EmailClass.GetMailParams(mailMessageBody, out fromEmailAddress, out toEmailAddress, out subject);
        mailMessageBody = mailMessageBody.Replace("{Credits}", membershipUser.UserName);
        try
        {
          MailMessage mailMessage = new MailMessage();
          mailMessage.IsBodyHtml = true;
          mailMessage.From = new MailAddress(fromEmailAddress);
          mailMessage.To.Add(membershipUser.UserName);
          mailMessage.Subject = subject;
          mailMessage.Body = mailMessageBody;
          SmtpClient smtpClient = new SmtpClient();
          smtpClient.Send(mailMessage);

        }
        catch (Exception ex)
        {
          // TODO: Create errorpage for user
          Response.Redirect("~/Pages/OrderSuccess.aspx");
        }
      }



			Session["businessPartnerId"] = bpId;
			Response.Redirect("~/Pages/OrderSuccess.aspx");
		}

		protected void plusButton_Click(object sender, EventArgs e)
		{
			decimal value = (decimal)creditsOrderTxtBox.Value;

			value = value + 50;
			creditsOrderTxtBox.Value = (double)value;
		}

		protected void minButton_Click(object sender, EventArgs e)
		{
			decimal value = (decimal)creditsOrderTxtBox.Value;
			if (value > 50)
			{
				value = value - 50;
				creditsOrderTxtBox.Value = (double)value;
			}
		}

		protected void usageReport_Click(object sender, EventArgs e)
		{
			string url = ResolveUrl("UsageReport.aspx?id=" + bpId);
			ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + url + "','_blank');", true);
		}
	}
}