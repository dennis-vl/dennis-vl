using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm
{
  public partial class ItemSelection : System.Web.UI.Page
  {
    private static DALPortalDataContext portalDc;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LinqDataSourceRadGrid_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      Guid membershipUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

      var q = from c in portalDc.shoppingCarts
              join d in portalDc.vw_items on new {a = c.companyCode, b = c.itemId} equals new {a = d.companyCode , b = d.itemId}
              where c.User.UserId.Equals(membershipUserId)
              select new { };

      List<shoppingCart> shoppingCarts = portalDc.shoppingCarts.Where(s => s.itemId == 1 && s.companyCode.Equals("")).ToList();
    }
  }
}
