using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages.Esm
{
  public partial class ItemOverview : BasePage
  {
    private static DALPortalDataContext portalDc;

    private ItemFilter itemFilter;

    private bool radGridCheckBoxSellectAllChecked;
    
    protected void Page_Load(object sender, EventArgs e)
    {
      portalDc = new DALPortalDataContext();

      if (!IsPostBack)
      {
        itemFilter = new ItemFilter();

        Session["itemFilter"] = itemFilter;
      }
      else
        itemFilter = (ItemFilter)Session["itemFilter"];
    }

    protected void DataSourceRadGrid_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      e.Result = EsmHelper.GetData(portalDc, this.itemFilter);
    }

    protected void LinqDataSourceAcbSize_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);
      
      e.Result = data.Select(c => new { c.size }).Distinct();
    }

    protected void LinqDataSourceAcbCompanyName_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.companyName }).Distinct();
    }

    protected void LinqDataSourceAcbDescription_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.description }).Distinct();
    }

    protected void LinqDataSourceAcbItemGroupName_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.itemGroupName }).Distinct();
    }

    protected void LinqDataSourceAcbOutsideDiameter1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.outsideDiameter1 }).Distinct();
    }

    protected void LinqDataSourceAcbOutsideTreatmentHeat_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.treatmentHeat }).Distinct();
    }

    protected void LinqDataSourceAcbOutsideDiameter2_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.outsideDiameter2 }).Distinct();
    }

    protected void LinqDataSourceAcbOutsideTreatmentSurface_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.treatmentSurface }).Distinct();
    }

    protected void LinqDataSourceAcbOutsideDiameter3_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.outsideDiameter3 }).Distinct();
    }

    protected void LinqDataSourceAcbEnds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.ends }).Distinct();
    }

    protected void LinqDataSourceAcbWallThickness1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.wallThickness1 }).Distinct();
    }

    protected void LinqDataSourceAcbCdi_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.cdi }).Distinct();
    }

    protected void LinqDataSourceAcbWallThickness2_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.wallThickness2 }).Distinct();
    }

    protected void LinqDataSourceAcbSupplier_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.supplier }).Distinct();
    }

    protected void LinqDataSourceAcbType_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.type }).Distinct();
    }

    protected void LinqDataSourceAcbOther_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.other }).Distinct();
    }

    protected void LinqDataSourceAcbSpecification_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.specification }).Distinct();
    }

    protected void LinqDataSourceAcbCertificates_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.certificates }).Distinct();
    }

    protected void LinqDataSourceAcbLength_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = EsmHelper.GetData(portalDc, this.itemFilter);

      e.Result = data.Select(c => new { c.length }).Distinct();
    }

    private void RefreshRadGridCertificate()
    {
      RadGridItemOverview.MasterTableView.SortExpressions.Clear();
      RadGridItemOverview.Rebind();
    }
    
    protected void RadAutoCompleteBoxSize_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.size = entries[0].Text;
      else
        this.itemFilter.size = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxCompany_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.companyName = entries[0].Text;
      else
        this.itemFilter.companyName = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxDescription_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.description = entries[0].Text;
      else
        this.itemFilter.description = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxArticleGroup_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.itemGroupName = entries[0].Text;
      else
        this.itemFilter.itemGroupName = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxOutsideDiameter1_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.outsideDiameter1 = entries[0].Text;
      else
        this.itemFilter.outsideDiameter1 = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxTreatmentHeat_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.treatmentHeat = entries[0].Text;
      else
        this.itemFilter.treatmentHeat = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxOutsideDiameter2_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.outsideDiameter2 = entries[0].Text;
      else
        this.itemFilter.outsideDiameter2 = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxTreatmentSurface_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.treatmentSurface = entries[0].Text;
      else
        this.itemFilter.treatmentSurface = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxOutsideDiameter3_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.outsideDiameter3 = entries[0].Text;
      else
        this.itemFilter.outsideDiameter3 = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxEnds_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.ends = entries[0].Text;
      else
        this.itemFilter.ends = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxWallThickness1_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.wallThickness1 = entries[0].Text;
      else
        this.itemFilter.wallThickness1 = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxCdi_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.cdi = entries[0].Text;
      else
        this.itemFilter.cdi = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxWallThickness2_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.wallThickness2 = entries[0].Text;
      else
        this.itemFilter.wallThickness2 = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxSupplier_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.supplier = entries[0].Text;
      else
        this.itemFilter.supplier = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxType_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.type = entries[0].Text;
      else
        this.itemFilter.type = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxOther_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.other = entries[0].Text;
      else
        this.itemFilter.other = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxSpecification_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.specification = entries[0].Text;
      else
        this.itemFilter.specification = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxCertificates_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.certificates = entries[0].Text;
      else
        this.itemFilter.certificates = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxLength_Entry(object sender, AutoCompleteEntryEventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
        this.itemFilter.length = entries[0].Text;
      else
        this.itemFilter.length = null;

      Session["itemFilter"] = this.itemFilter;

      RefreshRadGridCertificate();
    }

    protected void ImageButtonDetail_Click(object sender, ImageClickEventArgs e)
    {
      ImageButton imageButtonDetail = sender as ImageButton;
      GridDataItem gridDataItem = imageButtonDetail.NamingContainer as GridDataItem;

      int itemId = Convert.ToInt32(gridDataItem.GetDataKeyValue("itemId"));
      string companyCode = gridDataItem.GetDataKeyValue("companyCode").ToString();

      Session["selectedItemId"] = itemId;
      Session["selectedCompanyCode"] = companyCode;

      Response.Redirect("~/Pages/Esm/ItemDetail.aspx");
    }

    protected void RadButtonSelection_Click(object sender, EventArgs e)
    {
      if (RadGridItemOverview.SelectedItems.Count != 0) // no check boxes selected
      {
        List<int> rowIndexes = new List<int>();

        if (radGridCheckBoxSellectAllChecked) // all check box selected
        {

        }
        else // some check boxes selected
        {
          Guid membershipUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

          GridItemCollection gridItemCollection = RadGridItemOverview.SelectedItems;

          shoppingCart sc;

          foreach (GridDataItem data in gridItemCollection)
          {
            if (portalDc.shoppingCarts.Where(s => s.itemId == Convert.ToInt32(data.GetDataKeyValue("itemId"))
              && s.companyCode.Equals(data.GetDataKeyValue("companyCode").ToString())
              && s.userId.Equals(membershipUserId)
              && s.applicationCode.Equals("ESM")).IsNullOrEmpty())
            {
              sc = new shoppingCart
              {
                itemId = Convert.ToInt32(data.GetDataKeyValue("itemId")),
                companyCode = data.GetDataKeyValue("companyCode").ToString(),
                userId = membershipUserId,
                applicationCode = "ESM"
              };

              portalDc.shoppingCarts.InsertOnSubmit(sc);
            }
          }
        }

        // Submit the change to the database. 
        try
        {
          portalDc.SubmitChanges();
        }
        catch (Exception exception)
        {
          //
        }
      }
      else
      {
        // literal
      }
    }

    protected void RadGridItemOverview_ItemCreated(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridHeaderItem)
      {
        GridHeaderItem headerItem = (GridHeaderItem)e.Item;
        CheckBox checkBox = (CheckBox)headerItem["gridClientSelectColumn"].Controls[0];
        checkBox.AutoPostBack = true;
        checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
      }
    }

    void checkBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox)sender;

      if (checkBox.Checked)
        ViewState["radGridCheckBoxSellectAllChecked"] = true;
      else
        ViewState["radGridCheckBoxSellectAllChecked"] = false;
    }
  }
}
