using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Account
{
  public partial class Login : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // else the requested page will show after login
      if (Request.QueryString.Count != 0)
        Response.Redirect("~/Account/Login.aspx");
    }

		protected void LoginBox_LoggedIn(object sender, EventArgs e)
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			MembershipUser member = Membership.GetUser(LoginBox.UserName);

			string id = member.ProviderUserKey.ToString().ToLower();

			List<userRole> roles = dc.userRoles.Where(c => c.userId.ToString().ToLower().Equals(id)).ToList();

			if (roles.Any(c => c.roleCode.Equals("CERT_USER")) || roles.Any(c => c.roleCode.Equals("CERT_ADMIN")))
				Response.Redirect("~/Pages/DeliveryDetails.aspx");

			if (roles.Any(c => c.roleCode.Equals("DOP_USER")) || roles.Any(c => c.roleCode.Equals("DOP_ADMIN")))
				Response.Redirect("~/Pages/DOP/DOPDetails.aspx");
		}
  }
}
