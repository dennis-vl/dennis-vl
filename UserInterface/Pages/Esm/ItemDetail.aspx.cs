using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm
{
  public partial class ItemDetail : System.Web.UI.Page
  {
    private static DALPortalDataContext portalDc;

    private ItemFilter itemFilter;

    private int selectedItemId = -1;
    private string selectedCompanyCode = null;
    
    private int pageIndex = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
      portalDc = new DALPortalDataContext();

      this.itemFilter = (ItemFilter)Session["itemFilter"];
      
      if (!IsPostBack)
      {
        this.selectedItemId = Convert.ToInt32(Session["selectedItemId"]);
        this.selectedCompanyCode = Session["selectedCompanyCode"].ToString();

        IQueryable<vw_item> data = EsmHelper.GetData(portalDc, this.itemFilter);

        int index = data.ToList().FindIndex(c => c.itemId.Equals(selectedItemId) && c.companyCode.Equals(selectedCompanyCode));

        RadDataPager radDataPager = RadListViewItem.FindControl("RadDataPagerItem") as RadDataPager;
        radDataPager.FireCommand("Page", (index).ToString());
      }
    }
    
    protected void LinqDataSourceRadListViewItem_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      e.Result = EsmHelper.GetData(portalDc, this.itemFilter);
    }

    protected void LinqDataSourceRadGridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      RadDataPager radDataPager = RadListViewItem.FindControl("RadDataPagerItem") as RadDataPager;

      vw_item vwItem = data.ToList()[pageIndex];

      var query = from c in portalDc.purchaseOrderLines
                  where c.itemId == vwItem.itemId && c.purchaseOrder.company.companyCode == vwItem.companyCode
                  select new { c.openQuantity, c.uomCode, c.deliveryDate, c.purchaseOrder.businessPartner.bpName, c.purchaseOrder.businessPartner.country.description };

      e.Result = query;
    }

    protected void RadDataPagerItem_PageIndexChanged(object sender, RadDataPagerPageIndexChangeEventArgs e)
    {
      this.pageIndex = e.NewPageIndex;

      RadGridPurchaseOrder.MasterTableView.SortExpressions.Clear();
      RadGridPurchaseOrder.MasterTableView.GroupByExpressions.Clear();
      RadGridPurchaseOrder.Rebind();
    }
  }
}
