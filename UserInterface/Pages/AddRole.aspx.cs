using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface
{
	public partial class AddRole : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string username = (string)Session["username"];
			string name = (string)Session["name"];
			UsernameLiteral2.Text = username;
			if (name.Equals("&nbsp;"))
			{
				name = "";
			}
			if (!IsPostBack)
			{
				TextBoxName.Text = name;
			}

			DALPortalDataContext dc = new DALPortalDataContext();
			Guid userId2 = dc.Users.Where(c => c.UserName.Equals(username)).Select(c => c.UserId).SingleOrDefault();
			userSetting setting = dc.userSettings.Where(c => c.userId.Equals(userId2)).SingleOrDefault();
			if (setting != null)
			{
				companyDDL.SelectedValue = setting.company.companyCode;
				if (!IsPostBack)
				{
					adminCheckBox.Checked = setting.siteAdmin;
				}
			}
		}

		private IQueryable<string> GetDataUserRoles()
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			string username = (string)Session["username"];


			Guid userId2 = dc.Users.Where(c => c.UserName.Equals(username)).Select(c => c.UserId).SingleOrDefault();


			return dc.userRoles.Where(c => c.userId.Equals(userId2)).Select(c => c.roleCode);
		}

		private IQueryable<applicationRole> GetData()
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			string value = RadListBoxApplications.SelectedValue;

			return dc.applicationRoles.Where(c => c.application.applicationName.Equals(value));
		}

		protected void RadButtonCreate_Click(object sender, EventArgs e)
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			string username = (string)Session["username"];
			string name = (string)Session["name"];

			string[] items = new string[RadListBoxDestination.Items.Count];

			for (int i = 0; RadListBoxDestination.Items.Count > i; i++) //RadListBoxItem item in RadListBoxDestination.Items)
			{
				items[i] = RadListBoxDestination.Items[i].Value;
			}

			Guid userId2 = dc.Users.Where(c => c.UserName.Equals(username)).Select(c => c.UserId).SingleOrDefault();
			List<userRole> roles = dc.userRoles.Where(c => c.userId.Equals(userId2)).ToList();

			for (int i = 0; roles.Count() > i; i++)
			{
				if (!items.Contains(roles[i].roleCode))
				{
					//   roles.RemoveAt(i);
					dc.userRoles.DeleteOnSubmit(roles[i]);
				}
			}

			for (int i = 0; items.Count() > i; i++)
			{
				if (!roles.Any(c => c.roleCode.Equals(items[i])))
				{
					userRole roleUser = new userRole { userId = userId2, roleCode = items[i] };
					dc.userRoles.InsertOnSubmit(roleUser);
				}
			}

			userSetting userSetting = dc.userSettings.Where(c => c.userId.Equals(userId2)).SingleOrDefault();

			if (userSetting == null)
			{
				userSetting = new userSetting { userId = userId2 };
				userSetting.companyCode = "ZW";
				userSetting.defaultCultureCode = "nl";
				userSetting.name = TextBoxName.Text;
				userSetting.siteAdmin = adminCheckBox.Checked;
				dc.userSettings.InsertOnSubmit(userSetting);
			}
			else
			{
				userSetting.siteAdmin = adminCheckBox.Checked;
				userSetting.name = TextBoxName.Text;
				userSetting.companyCode = companyDDL.SelectedValue;
			}

			dc.SubmitChanges();
			Response.Redirect("~/Pages/UserAccounts.aspx");

		}

		protected void RadListBoxApplications_SelectedIndexChanged(object sender, EventArgs e)
		{
			RadListBoxSource.DataBind();
		}

		protected void rolesDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			e.Result = GetData();

			DALPortalDataContext dc = new DALPortalDataContext();

			string username = (string)Session["username"];

			Guid userId2 = dc.Users.Where(c => c.UserName.Equals(username)).Select(c => c.UserId).SingleOrDefault();

			List<string> roles = dc.userRoles.Where(c => c.userId.Equals(userId2)).Select(c => c.roleCode).ToList();


			for (int i = 0; roles.Count() > i; i++)
			{
				if (RadListBoxSource.FindItemByValue(roles[i]) != null)
				{
					RadListBoxSource.FindItemByValue(roles[i]).Remove();
				}
			}
			RadListBox items = RadListBoxApplications;

			string valueListBox = RadListBoxApplications.SelectedValue;
		}

		protected void rolesDestinationDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			e.Result = GetDataUserRoles();
		}

		protected void RadButton1_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Pages/UserAccounts.aspx");
		}
	}
}