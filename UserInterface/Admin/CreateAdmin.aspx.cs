using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Admin
{
  public partial class CreateAdmin : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    // protected void RadListBox1_SelectedIndexChanged(object sender, EventArgs e)


    protected void RadButtonCreate_Click(object sender, EventArgs e)
    {
      if (Membership.FindUsersByName(TextBoxUserName.Text).Count == 0 && Membership.FindUsersByEmail(TextBoxUserName.Text).Count == 0)
      {
        MembershipUser membershipUser = Membership.CreateUser(TextBoxUserName.Text, TextBoxPassword.Text, TextBoxUserName.Text);
        Roles.AddUserToRole(TextBoxUserName.Text, "User");
        Roles.AddUserToRole(TextBoxUserName.Text, "Administrator");

        PlaceHolderCreateAdmin.Visible = false;
        CreateAdminStatus.Text = "Admin user " + TextBoxUserName.Text + " is created";
      }
      else
        CreateAdminStatus.Text = "There is already a user with that username";
    }

    protected void RadListBoxApplications_SelectedIndexChanged1(object sender, EventArgs e)
    {
        string value = RadListBoxApplications.SelectedValue;
        rolesDataSource.WhereParameters.Add("applicationCode", value);
    }
  }
}