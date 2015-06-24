using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface
{
	public partial class CreateUser : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{


		}

		protected void RadButtonCreate_Click(object sender, EventArgs e)
		{
			if (Membership.FindUsersByName(TextBoxUserName.Text).Count == 0 && Membership.FindUsersByEmail(TextBoxUserName.Text).Count == 0)
			{
				MembershipUser membershipUser = Membership.CreateUser(TextBoxUserName.Text, TextBoxPassword.Text, TextBoxUserName.Text);

				Roles.AddUserToRole(TextBoxUserName.Text, "User");
				//        Roles.AddUserToRole(TextBoxUserName.Text, "Administrator");

				PlaceHolderCreateAdmin.Visible = false;
				CreateAdminStatus.Text = "User account " + TextBoxUserName.Text + " is created";

				DALPortalDataContext dc = new DataAccess.Database.DALPortalDataContext();
				userSetting setting = new userSetting();
				setting.userId = (Guid)membershipUser.ProviderUserKey;
				setting.name = TextBoxName.Text;
				setting.companyCode = companyDDL.SelectedValue;
				setting.defaultCultureCode = "nl";

				dc.userSettings.InsertOnSubmit(setting);
				dc.SubmitChanges();
				Session["username"] = TextBoxUserName.Text;
				Session["name"] = TextBoxName.Text;
				Response.Redirect("~/Pages/AddRole.aspx");
			}
			else
				CreateAdminStatus.Text = "There is already a user with that username";
		}
	}
}