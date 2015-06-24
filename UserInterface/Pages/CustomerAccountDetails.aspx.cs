using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class CustomerAccountDetails : BasePage
	{

		private static DALPortalDataContext dc;
		private string bpCode;
		private Int32 businessPartnerId;
		private bool isMother;
		protected void Page_Load(object sender, EventArgs e)
		{
			bpCode = (string)Session["bpCode"];
			companyCode2.Text = bpCode;
			businessPartnerId = (Int32)Session["businessPartnerId"];
			dc = new DALPortalDataContext();

			string bpName = dc.businessPartners.Where(c => c.bpCode.Equals(bpCode)).Select(c => c.bpName).SingleOrDefault();
			string country = dc.businessPartners.Where(c => c.bpCode.Equals(bpCode)).Select(c => c.countryCode).SingleOrDefault();

			company2.Text = bpName;
			LabelCountry2.Text = country;

			bool isMother = checkBoxMother.Checked;
			if (!IsPostBack)
			{
				checkBoxMother.Checked = getCheckBoxState();
				isMother = getCheckBoxState();
				accountManagerDDL.SelectedValue = dc.businessPartners.Where(c => c.bpCode.Equals(bpCode)).Select(c => c.accountManager).SingleOrDefault().ToString();
				var isSister = dc.businessPartners.Where(c => c.bpCode.Equals(bpCode)).Select(c => c.bpMother).SingleOrDefault();

				if (isSister > 0)
				{
					selectSistersButton.Enabled = false;
					dopCheckBox.Enabled = false;
					certificatesCheckBox.Enabled = false;
					ImageButton1.Enabled = false;
					selectSistersButton.Enabled = false;
					checkBoxMother.Enabled = false;
				}
			}

			if (!isMother)
			{
				selectSistersButton.Enabled = false;
			}
			else
			{
				selectSistersButton.Enabled = true;
			}
			if (!IsPostBack)
			{
				var bpRoles = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Select(c => c).ToList();
				foreach (var role in bpRoles)
				{
					businessPartnerApplication roleE = role;
					if (roleE.applicationCode.Equals("CERT"))
					{
						certificatesCheckBox.Checked = true;
					}
					if (roleE.applicationCode.Equals("DOP"))
					{
						dopCheckBox.Checked = true;
					}
				}
			}
		}

		private bool getCheckBoxState()
		{
			bool isMother = dc.businessPartners.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Select(c => c.isMother).SingleOrDefault();
			return isMother;
		}

		protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{

			e.Result = dc.Users.Where(c => c.userRoles.Any(d => d.roleCode.Equals("CERT_ACTMNG"))).Select(t => new { t.userSetting.userId, t.userSetting.name });
		}

		protected void selectSistersButton_Click(object sender, EventArgs e)
		{
			businessPartner bp = dc.businessPartners.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Select(c => c).SingleOrDefault();
			var bpRolesList = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(businessPartnerId));
			foreach (var detail in bpRolesList)
			{
				dc.businessPartnerApplications.DeleteOnSubmit(detail);
			}
			if (certificatesCheckBox.Checked)
			{
				businessPartnerApplication bpRoleCert = new businessPartnerApplication();
				bpRoleCert.businessPartnerId = businessPartnerId;
				bpRoleCert.applicationCode = "CERT";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleCert);
			}

			if (dopCheckBox.Checked)
			{
				businessPartnerApplication bpRoleDop = new businessPartnerApplication();
				bpRoleDop.businessPartnerId = businessPartnerId;
				bpRoleDop.applicationCode = "DOP";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleDop);
			}
			bp.isMother = checkBoxMother.Checked;
			bp.accountManager = new Guid(accountManagerDDL.SelectedValue);
			dc.SubmitChanges();

			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/SelectSisters.aspx");
		}

		protected void saveButton_Click(object sender, EventArgs e)
		{
			businessPartner bp = dc.businessPartners.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Select(c => c).SingleOrDefault();
			var bpRolesList = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(businessPartnerId));
			foreach (var detail in bpRolesList)
			{
				dc.businessPartnerApplications.DeleteOnSubmit(detail);
			}
			if (certificatesCheckBox.Checked)
			{
				businessPartnerApplication bpRoleCert = new businessPartnerApplication();
				bpRoleCert.businessPartnerId = businessPartnerId;
				bpRoleCert.applicationCode = "CERT";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleCert);
			}

			if (dopCheckBox.Checked)
			{
				businessPartnerApplication bpRoleDop = new businessPartnerApplication();
				bpRoleDop.businessPartnerId = businessPartnerId;
				bpRoleDop.applicationCode = "DOP";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleDop);
			}
			bp.isMother = checkBoxMother.Checked;
			bp.accountManager = new Guid(accountManagerDDL.SelectedValue);
			dc.SubmitChanges();
			Response.Redirect("~/Pages/CustomerAccounts.aspx");
		}

		protected void cancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Pages/CustomerAccounts.aspx");
		}

		protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
		{
			businessPartner bp = dc.businessPartners.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Select(c => c).SingleOrDefault();
			var bpRolesList = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(businessPartnerId));
			foreach (var detail in bpRolesList)
			{
				dc.businessPartnerApplications.DeleteOnSubmit(detail);
			}
			if (certificatesCheckBox.Checked)
			{
				businessPartnerApplication bpRoleCert = new businessPartnerApplication();
				bpRoleCert.businessPartnerId = businessPartnerId;
				bpRoleCert.applicationCode = "CERT";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleCert);
			}

			if (dopCheckBox.Checked)
			{
				businessPartnerApplication bpRoleDop = new businessPartnerApplication();
				bpRoleDop.businessPartnerId = businessPartnerId;
				bpRoleDop.applicationCode = "DOP";
				dc.businessPartnerApplications.InsertOnSubmit(bpRoleDop);
			}
			bp.isMother = checkBoxMother.Checked;
			bp.accountManager = new Guid(accountManagerDDL.SelectedValue);
			dc.SubmitChanges();
			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/CertficateModelDetails.aspx");
		}



	}
}