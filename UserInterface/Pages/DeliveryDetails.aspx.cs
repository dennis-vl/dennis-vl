using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
  public partial class DeliveryDetails : BasePage
  {
    private static DALPortalDataContext dc;

    private string salesOrderNumber = null;
    private int salesOrderLineNumber = -1;
    private string deliveryNumber = null;
    private int deliveryLineNumber = -1;
    private string customerReference = null;
    private string salesOrderLineReference = null;
    private string itemCode = null;
    private string itemDescription = null;
    private string heatNumber = null;

    private string cardCode = null;

    private bool radGridCheckBoxSellectAllChecked = false;

    protected void Page_Load(object sender, EventArgs e)
    {
      dc = new DALPortalDataContext();

      if (UserIsAdmin())
      {
        if (ViewState["cardCode"] != null)
          cardCode = ViewState["cardCode"].ToString();

        string columnUniqueName = "CardCode";
        if (!RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible)
        {
          RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible = true;
        }

        AddCardCodeRowToFilterTable();
      }

      string userName = Membership.GetUser().UserName;
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(Membership.GetUser().ProviderUserKey)).Select(c => c.businessPartnerId).SingleOrDefault();
      if (dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Any())
      {
        if (dc.businessPartners.Where(c => c.bpMother.Equals(bpId)).Select(c => c.bpCode).Any())
        {
          if (ViewState["cardCode"] != null)
            cardCode = ViewState["cardCode"].ToString();

          string columnUniqueName = "bpName";
          if (!RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible)
          {
            RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible = true;
          }
        }
      }
      businessPartner bPartner;
      if (dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).Any())
      {
        bPartner = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).SingleOrDefault();
        if (bPartner.bpMother != null)
        {
          if (ViewState["cardCode"] != null)
            cardCode = ViewState["cardCode"].ToString();

          string columnUniqueName = "bpName";
          if (!RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible)
          {
            RadGridCertificate.MasterTableView.Columns.FindByUniqueNameSafe(columnUniqueName).Visible = true;
          }
        }
      }


      if (IsPostBack)
      {
        if (ViewState["salesOrderNumber"] != null)
          salesOrderNumber = ViewState["salesOrderNumber"].ToString();

        if (ViewState["salesOrderLineNumber"] != null)
          salesOrderLineNumber = Convert.ToInt32(ViewState["salesOrderLineNumber"]);

        if (ViewState["deliveryNumber"] != null)
          deliveryNumber = ViewState["deliveryNumber"].ToString();

        if (ViewState["deliveryLineNumber"] != null)
          deliveryLineNumber = Convert.ToInt32(ViewState["deliveryLineNumber"]);

        if (ViewState["customerReference"] != null)
          customerReference = ViewState["customerReference"].ToString();

        if (ViewState["salesOrderLineReference"] != null)
          salesOrderLineReference = ViewState["salesOrderLineReference"].ToString();

        if (ViewState["itemCode"] != null)
          itemCode = ViewState["itemCode"].ToString();

        if (ViewState["itemDescription"] != null)
          itemDescription = ViewState["itemDescription"].ToString();

        if (ViewState["heatNumber"] != null)
          heatNumber = ViewState["heatNumber"].ToString();

        if (ViewState["radGridCheckBoxSellectAllChecked"] != null)
          radGridCheckBoxSellectAllChecked = (bool)ViewState["radGridCheckBoxSellectAllChecked"];
      }

      UpdateModelInfo();

      string value = Session["pageUrl"] as string;

      if (!String.IsNullOrEmpty(value))
      {
        string url = ResolveUrl(value);
        ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('" + url + "','_blank');", true);
        Session.Remove("pageUrl");
      }
    }

    private void UpdateModelInfo()
    {
      String warningText = String.Empty;
      Double daysLeft = 0;
      
      Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(userId)).Select(c => c.businessPartnerId).SingleOrDefault();
      int modelId = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(bpId)).Where(r => r.isActive.Equals(true)).Select(c => c.modelId).SingleOrDefault();
      
      certificateBundle bundle = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(bpId)).Where(r => r.isActive.Equals(true)).Select(c => c).SingleOrDefault();
      labelBpId.Text = bpId.ToString();
      labelBpId.Visible = true;

      var isSister = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c.bpMother).SingleOrDefault();
      if (isSister > 0)
      {
        labelBpId.Text = isSister.ToString();
        bundle = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(isSister)).Where(r => r.isActive.Equals(true)).Select(c => c).SingleOrDefault();
        modelId = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(isSister)).Where(r => r.isActive.Equals(true)).Select(c => c.modelId).SingleOrDefault();
      }

      switch (modelId)
      {
        case 1:

          updradeButton.Visible = false;
          usageReportBut.Visible = false;
          //GetLocalResourceObject("AccountActive").ToString();

         // statusModel.Text = "Your acccount is active until " + bundle.expireDate.Value.ToString("MMMM dd, yyyy") + ".";
          statusModel.Text = GetLocalResourceObject("AccountActive").ToString() + bundle.expireDate.Value.ToString("MMMM dd, yyyy") + ".";
          satusModelLit.Text = GetLocalResourceObject("AccountActive").ToString() + bundle.expireDate.Value.ToString("MMMM dd, yyyy") + ".";
          DateTime dateNow = DateTime.Now;
          daysLeft = (bundle.expireDate.Value - dateNow).TotalDays;
          if (daysLeft < 0)
          {
            if (String.IsNullOrEmpty(warningText))
            {
              warningText = GetLocalResourceObject("AccountActive").ToString();
            }
          }
          else if (daysLeft < 30)
          {
            if (warningText.Equals(""))
            {
              warningText = GetLocalResourceObject("BundleExpired").ToString();
            }
          }

          if (warningText.Equals(""))
          {
            warningLiteral.Visible = false;
          }
          else
          {
            warningLiteral.Text = warningText;
            warningLiteral.Visible = true;
          }
          break;



        case 2:

          statusModel.Text = GetLocalResourceObject("AccountActive").ToString() + bundle.expireDate.Value.ToString("MMMM dd, yyyy") + ".";
          satusModelLit.Text = GetLocalResourceObject("AccountActive").ToString() + bundle.expireDate.Value.ToString("MMMM dd, yyyy") + ".";
     
          if (bundle.actualCertQty < 11)
          {
            warningText = GetLocalResourceObject("AlmostOutOfCredits").ToString();
          }
          else if (bundle.actualCertQty < 1)
          {
            warningText = GetLocalResourceObject("OutOfCredits").ToString();
          }

          daysLeft = (bundle.expireDate.Value - DateTime.Now).TotalDays;
          if (daysLeft < 0)
          {
            if (warningText.Equals(""))
            {
              warningText = GetLocalResourceObject("BundleExpiredUpgradeForMore").ToString(); 
            }
            else
            {
              warningText = warningText + ". " + GetLocalResourceObject("BundleExpiredUpgradeForMore").ToString(); 
            }

          }
          else if (daysLeft < 30)
          {
            if (warningText.Equals(""))
            {
              warningText = GetLocalResourceObject("BundleAlmostExpiredUpgradeForMore").ToString(); 
            }
            else
            {
              warningText = warningText + ". " + GetLocalResourceObject("BundleAlmostExpiredUpgradeForMore").ToString(); 
            }
          }

          if (warningText.Equals(""))
          {
            warningLiteral.Visible = false;
          }
          else
          {
            warningLiteral.Text = warningText;
            warningLiteral.Visible = true;
          }
          break;
        case 3:
          statusModel.Text = GetLocalResourceObject("UnlimitedAcces").ToString();
          satusModelLit.Text = GetLocalResourceObject("UnlimitedAcces").ToString(); 
          updradeButton.Visible = false;
          usageReportBut.Visible = false;
          usageReportBut.Visible = false;
          break;
        case 4:
          if (bundle.usedCertQty == null)
          {
            statusModel.Text = GetLocalResourceObject("ZeroCreditsUsed").ToString();
            satusModelLit.Text = GetLocalResourceObject("ZeroCreditsUsed").ToString();
          }
          else
          {
            statusModel.Text = GetLocalResourceObject("UsedCredit").ToString() + bundle.usedCertQty.ToString() + GetLocalResourceObject("Credit").ToString();
            satusModelLit.Text = GetLocalResourceObject("UsedCredit").ToString() + bundle.usedCertQty.ToString() + GetLocalResourceObject("Credit").ToString();
          }
          updradeButton.Visible = false;
          usageReportBut.Visible = true;
          break;
        case 5:
          statusModel.Text = GetLocalResourceObject("CertificatedOrdered").ToString();
          satusModelLit.Text = GetLocalResourceObject("CertificatedOrdered").ToString();
          updradeButton.Visible = false;
          usageReportBut.Visible = false;
          break;
        default:
          if (!UserIsAdmin())
          {
            statusModel.Text = "No model active.";
            satusModelLit.Text = "No model active.";
          }
          updradeButton.Visible = false;
          usageReportBut.Visible = false;
          break;
      }

      var hasRole = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(bpId)).Where(c => c.applicationCode.Equals("CERT")).Select(c => c).SingleOrDefault();
      if (!UserIsAdmin())
      {
        if (hasRole == null)
        {
          if (isSister > 0)
          {
            var hasRole2 = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(isSister)).Where(c => c.applicationCode.Equals("CERT")).Select(c => c).SingleOrDefault();
            if (hasRole2 == null)
            {
              RadGridCertificate.Enabled = false;
              warningLiteral.Visible = true;
              warningLiteral.Text = GetLocalResourceObject("AccountDisabled").ToString();
            }
          }
          else
          {
            RadGridCertificate.Enabled = false;
            warningLiteral.Visible = true;
            warningLiteral.Text = GetLocalResourceObject("AccountDisabled").ToString();
          }
        }
      }
    }

    protected void UpdateCredits(int usedCredits, certificateBundle bundle)
    {
      if (bundle.modelId == 2)
      {
        int newCreds = (int)bundle.actualCertQty - usedCredits;
        bundle.actualCertQty = bundle.actualCertQty - usedCredits;
        
        if (bundle.usedCertQty == null)
          bundle.usedCertQty = usedCredits;
        else
          bundle.usedCertQty = bundle.usedCertQty + usedCredits;

        dc.SubmitChanges();
      }
      else if (bundle.modelId == 4)
      {
        if (bundle.usedCertQty == null)
          bundle.usedCertQty = usedCredits;
        else
          bundle.usedCertQty = bundle.usedCertQty + usedCredits;

        dc.SubmitChanges();
      }
    }

    private void AddCardCodeRowToFilterTable()
    {
      HtmlTable htmlTable = (HtmlTable)SearchPanelBar.Items[0].Items[0].FindControl("filterGridTable");

      HtmlTableRow htmlTableRow = new HtmlTableRow();

      HtmlTableCell htmlTableCell = new HtmlTableCell();
      htmlTableCell.InnerText = GetLocalResourceObject("CustomerCode").ToString();
      htmlTableRow.Cells.Add(htmlTableCell);

      RadAutoCompleteBox radAutoCompleteBox = new RadAutoCompleteBox();
      radAutoCompleteBox.ID = "RadAutoCompleteBoxCardCode";
      radAutoCompleteBox.EmptyMessage = GetLocalResourceObject("CustomerCode").ToString();
      radAutoCompleteBox.DataSourceID = "AutoCompleteBoxCardCodeDataSource";
      radAutoCompleteBox.DataTextField = "CardCode";
      radAutoCompleteBox.InputType = RadAutoCompleteInputType.Token;
      radAutoCompleteBox.EntryAdded += RadAutoCompleteBoxCardCode_Event;
      radAutoCompleteBox.EntryRemoved += RadAutoCompleteBoxCardCode_Event;
      radAutoCompleteBox.AutoPostBack = true;
      radAutoCompleteBox.Width = 400;
      radAutoCompleteBox.DropDownWidth = 400;
      radAutoCompleteBox.OnClientRequesting = "NoCharactersAfterOneEntryBlock";

      radAutoCompleteBox.TextSettings.SelectionMode = RadAutoCompleteSelectionMode.Single;

      htmlTableCell = new HtmlTableCell();
      htmlTableCell.Controls.Add(radAutoCompleteBox);
      htmlTableRow.Cells.Add(htmlTableCell);

      htmlTable.Rows.Add(htmlTableRow);

      //htmlTable.Rows.Insert(0, htmlTableRow);

      //HtmlTableRow[] htmlTableRows = new HtmlTableRow[htmlTable.Rows.Count + 1];
      //htmlTableRows[0] = htmlTableRow;

      //for (int i = 0; i < htmlTable.Rows.Count; i++)
      //  htmlTableRows[i + 1] = htmlTable.Rows[i];

      //htmlTable.Rows.Clear();

      //foreach (HtmlTableRow htr in htmlTableRows)
      //  htmlTable.Rows.Add(htr);
    }

    private bool UserIsAdmin()
    {
      Guid id = (Guid)Membership.GetUser().ProviderUserKey;

      List<userRole> roles = dc.userRoles.Where(c => c.userId.Equals(id)).Select(c => c).ToList();
      foreach (userRole role in roles)
      {
        if (role.roleCode.Equals("CERT_VLUSER"))
        {
          return true;
        }
        if (role.roleCode.Equals("CERT_ACTMNG"))
        {
          return true;
        }
      }

      userSetting setting = dc.userSettings.Where(c => c.userId.Equals(id)).Select(c => c).SingleOrDefault();
      if (setting != null)
        if (setting.siteAdmin)
          return true;

      return false;
    }

    protected void OpenPDF(object sender, EventArgs e)
    {
      LinkButton lnk = (LinkButton)sender;
      if (lnk != null)
      {
        Response.AddHeader("content-disposition", "attachment; filename=" + lnk.CommandArgument);
        Response.WriteFile(lnk.CommandArgument.ToString());
        Response.End();
      }
    }


    private static List<String> GetBpCodes()
    {
      string userName = Membership.GetUser().UserName;
      List<String> mainList = new List<String>();
      List<String> subList = new List<String>();
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(Membership.GetUser().ProviderUserKey)).Select(c => c.businessPartnerId).SingleOrDefault();
      if (dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Any())
      {
        //return dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Select(c => c.businessPartner.bpCode).ToList();
        businessPartner bPartner = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).SingleOrDefault();
        mainList = dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Select(c => c.businessPartner.bpCode).ToList();
        if (bPartner.bpMother != null)
        {
          bpId = (int)bPartner.bpMother;
          mainList = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c.bpCode).ToList();
        }

        if (dc.businessPartners.Where(c => c.bpMother.Equals(bpId)).Select(c => c.bpCode).Any())
        {
          subList = dc.businessPartners.Where(c => c.bpMother.Equals(bpId)).Select(c => c.bpCode).ToList();

          mainList.AddRange(subList);
        }
        return mainList;
      }
      else
      {
        return new List<String>();
      }

    }

    private static List<String> GetBpNames()
    {
      string userName = Membership.GetUser().UserName;
      List<String> mainList = new List<String>();
      List<String> subList = new List<String>();
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(Membership.GetUser().ProviderUserKey)).Select(c => c.businessPartnerId).SingleOrDefault();
      if (dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Any())
      {
        //return dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Select(c => c.businessPartner.bpCode).ToList();
        businessPartner bPartner = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).SingleOrDefault();
        mainList = dc.contactPersons.Where(c => c.eMail.ToLower().Equals(userName.ToLower())).Select(c => c.businessPartner.bpCode).ToList();
        if (bPartner.bpMother != null)
        {
          bpId = (int)bPartner.bpMother;
          mainList = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c.bpName).ToList();
        }

        if (dc.businessPartners.Where(c => c.bpMother.Equals(bpId)).Select(c => c.bpCode).Any())
        {
          subList = dc.businessPartners.Where(c => c.bpMother.Equals(bpId)).Select(c => c.bpName).ToList();

          mainList.AddRange(subList);
        }
        return mainList;
      }
      else
      {
        return new List<String>();
      }

    }

    protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      e.Result = GetData();
    }

    protected void AutoCompleteBoxSONDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.SODocNum }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxSOLNDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.SOLineNum }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxDNDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.DELDocNum }).Distinct();

      e.Result = result;
    }

    protected void RadAutoCompleteBoxDLNDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.DELLineNum }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxCRDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.customerReference }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxSOLRDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.SOLineReference }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxIDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      //  var result = data.Select(c => new { ItemCode = c.itemCode + "  -  " + c.itemDescription }).Distinct();
      var result = data.Select(c => new { ItemCode = c.itemCode }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxHNDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.HeatNumber }).Distinct();

      e.Result = result;
    }

    protected void AutoCompleteBoxCardCodeDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      var data = GetData();
      var result = data.Select(c => new { c.CardCode }).Distinct();

      e.Result = result;
    }

    private IQueryable<vw_DeliveryLine> GetData()
    {
      var predicate = PredicateExtensions.True<vw_DeliveryLine>();

      if (!UserIsAdmin())
      {
        List<String> bpCodes = GetBpCodes();
        predicate = predicate.And(c => bpCodes.Contains(c.CardCode));
      }


      if (!String.IsNullOrEmpty(salesOrderNumber))
        predicate = predicate.And(c => c.SODocNum.Equals(salesOrderNumber));
      if (salesOrderLineNumber > -1)
        predicate = predicate.And(c => c.SOLineNum.Equals(salesOrderLineNumber));
      if (!String.IsNullOrEmpty(deliveryNumber))
        predicate = predicate.And(c => c.DELDocNum.Equals(deliveryNumber));
      if (deliveryLineNumber > -1)
        predicate = predicate.And(c => c.DELLineNum.Equals(deliveryLineNumber));
      if (!String.IsNullOrEmpty(customerReference))
        predicate = predicate.And(c => c.customerReference.Equals(customerReference));
      if (!String.IsNullOrEmpty(salesOrderLineReference))
        predicate = predicate.And(c => c.SOLineReference.Equals(salesOrderLineReference));
      if (!String.IsNullOrEmpty(itemCode))
        predicate = predicate.And(c => c.itemCode.Equals(itemCode));

      if (!String.IsNullOrEmpty(heatNumber))
        predicate = predicate.And(c => c.HeatNumber.Equals(heatNumber));

      if (!String.IsNullOrEmpty(cardCode))
        predicate = predicate.And(c => c.CardCode.Equals(cardCode));



      Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(userId)).Select(c => c.businessPartnerId).SingleOrDefault();
      int modelId = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(bpId)).Where(r => r.isActive.Equals(true)).Select(c => c.modelId).SingleOrDefault();

      if (modelId == 5)
      {
        predicate = predicate.And(c => c.certOrdered.Equals(1));
      }

      //dc.vw_DeliveryLines.Where(c => c.batchId)
      return dc.vw_DeliveryLines.Where(predicate).OrderByDescending(c => c.DELDocDate).ThenByDescending(d => d.DELDocNum).ThenBy(d => d.DELLineNum);
    }

    protected void RadAutoCompleteBoxSalesOrderNumber_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.salesOrderNumber = entries[0].Text;
        ViewState["salesOrderNumber"] = entries[0].Text;
      }
      else
      {
        this.salesOrderNumber = null;
        ViewState["salesOrderNumber"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxDeliveryNumber_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.deliveryNumber = entries[0].Text;
        ViewState["deliveryNumber"] = entries[0].Text;
      }
      else
      {
        this.deliveryNumber = null;
        ViewState["deliveryNumber"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxCustomerReference_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.customerReference = entries[0].Text;
        ViewState["customerReference"] = entries[0].Text;
      }
      else
      {
        this.customerReference = null;
        ViewState["customerReference"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxSalesOrderLineReference_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.salesOrderLineReference = entries[0].Text;
        ViewState["salesOrderLineReference"] = entries[0].Text;
      }
      else
      {
        this.salesOrderLineReference = null;
        ViewState["salesOrderLineReference"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxItem_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        string[] stringSeparators = new string[] { "  -  " };
        string[] result = entries[0].Text.ToString().Split(stringSeparators, StringSplitOptions.None);

        this.itemCode = result[0];
        ViewState["itemCode"] = result[0];
        //this.itemDescription = result[1];
        //  ViewState["itemDescription"] = result[1];
      }
      else
      {
        this.itemCode = null;
        ViewState["itemCode"] = null;
        // this.itemDescription = null;
        //  ViewState["itemDescription"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxHeatNumber_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.heatNumber = entries[0].Text;
        ViewState["heatNumber"] = entries[0].Text;
      }
      else
      {
        this.heatNumber = null;
        ViewState["heatNumber"] = null;
      }

      RefreshRadGridCertificate();
    }

    protected void RadAutoCompleteBoxCardCode_Event(object sender, EventArgs e)
    {
      AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
      if (entries.Count != 0)
      {
        this.cardCode = entries[0].Text;
        ViewState["cardCode"] = entries[0].Text;
      }
      else
      {
        this.cardCode = null;
        ViewState["cardCode"] = null;
      }

      RefreshRadGridCertificate();
    }

    private void RefreshRadGridCertificate()
    {
      // Kunnen weg?
      RadGridCertificate.MasterTableView.SortExpressions.Clear();
      RadGridCertificate.MasterTableView.GroupByExpressions.Clear();
      RadGridCertificate.MasterTableView.CurrentPageIndex = 0;

      // Moet blijven
      RadGridCertificate.Rebind();
    }

    private List<string> GetExistingCertificateLinks(List<String> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      foreach (string certificateLink in certificateLinks)
      {
        if (File.Exists(certificateLink))
          existingCertificates.Add(certificateLink);
      }

      return existingCertificates;
    }

    private List<string> GetExistingHeats(List<vw_DeliveryLine> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add(certificateLinks[i].HeatNumber);
      }

      return existingCertificates;
    }

    private List<string> GetExistingComCodes(List<vw_DeliveryLine> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add(certificateLinks[i].CardCode);
      }

      return existingCertificates;
    }

    private List<string> GetExistingDociD(List<vw_DeliveryLine> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add(certificateLinks[i].SODocNum);
      }

      return existingCertificates;
    }

    private List<string> GetExistingDelDocNum(List<vw_DeliveryLine> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add(certificateLinks[i].DELDocNum);
      }

      return existingCertificates;
    }

    private List<int> GetExistingDelLineNum(List<vw_DeliveryLine> certificateLinks)
    {
      List<int> existingCertificates = new List<int>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add((int)certificateLinks[i].DELLineNum);
      }

      return existingCertificates;
    }

    private List<int> GetExistingBatchId(List<vw_DeliveryLine> certificateLinks)
    {
      List<int> existingCertificates = new List<int>();
      for (int i = 0; i < certificateLinks.Count; i++)
      {
        string exist = HttpContext.Current.Server.MapPath(certificateLinks[i].CertificateLink);
        if (File.Exists(exist))
          existingCertificates.Add((int)certificateLinks[i].batchId);
      }

      return existingCertificates;
    }

    private static string FilenameAndExtension(string extension)
    {
      string timeCreated = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year +
        " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second;
      return GetBpCodes().FirstOrDefault() + " " + timeCreated + extension;
    }


    protected void RadButtonMergePDFs_Click(object sender, EventArgs e)
    {
      List<vw_DeliveryLine> selectedLines = GetSelectedVwLines();
      

      if (selectedLines.Count != 0)
      {
        Int32 usedCreds = selectedLines.Count();
        Double daysLeft = 0;
        Int32 newCredsUsed = 0;

        Guid userId = GetUserId();
        contactPerson contactPerson = ContactPersonClass.GetContactPerson(userId, dc);
        businessPartner bpMother = new businessPartner();
        certificateBundle currentBundle = new certificateBundle() { modelId = 0 };

        if (contactPerson != null)
        {
          bpMother = BusinessPartnerClass.GetBusinessPartnerMother(userId, dc);
          currentBundle = CertificateBundleClass.GetCurrentBundle(bpMother.businessPartnerId, dc);
        }

        switch (currentBundle.modelId)
        {
          // No Model
          case 0:
            List<userRole> roles = dc.userRoles.Where(c => c.userId.Equals(userId)).ToList();
            Boolean siteAdmin = dc.userSettings.Any(c => c.siteAdmin && c.userId.Equals(userId));
            if (siteAdmin || roles.Any(c => c.roleCode.Equals("CERT_ACTMNG")) || roles.Any(c => c.roleCode.Equals("CERT_VLUSER")))
            {
              CreatePdf(selectedLines);
            }
            else
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('No bundle active.');", true);
            }

            break;

          // One Year Access
          case 1:

            daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;

            if (daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
            }
            else
            {
              CreatePdf(selectedLines);
              newCredsUsed = CalculateCreditsUsed(selectedLines);
              RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
              UpdateCredits(newCredsUsed, currentBundle);
            }
            break;

          // Certificate Credit x times 50
          case 2:

            newCredsUsed = CalculateCreditsUsed(selectedLines);
            daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;

            int certCount = currentBundle.actualCertQty.Value;
            if (certCount < 1 && daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits and the bundle as expired.');", true);
            }
            else if (certCount < newCredsUsed)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits.');", true);
            }
            else if (daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
            }
            else
            {
              CreatePdf(selectedLines);
              RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
              UpdateCredits(newCredsUsed, currentBundle);
            }
            break;
          
          // All other Models
          default:
            CreatePdf(selectedLines);
            newCredsUsed = CalculateCreditsUsed(selectedLines);
            RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
            UpdateCredits(newCredsUsed, currentBundle);
            break;
        }

        UpdateModelInfo();
      }
      else
        LiteralNoCertificatesSelected.Visible = true;
    }

    private int CalculateCreditsUsed(List<vw_DeliveryLine> selectedLines)
    {
      List<String> lines = selectedLines.Select(c => new { line = c.batchId.Value.ToString() + "&" + c.DELDocId.ToString() + "&" + c.DELLineNum.ToString() }).Select(c => c.line).ToList();
      Int32 alreadyDownloaded = dc.usageReports.Where(c => lines.Contains(c.batchId.Value.ToString() + "&" + c.baseDocId.ToString() + "&" + c.baseLineNum.ToString())).Select(c => new { c.batchId, c.baseDocId, c.baseLineNum }).Distinct().Count();

      return selectedLines.Count - alreadyDownloaded;
    }

    private void CreatePdf(List<vw_DeliveryLine> selectedLines)
    {
      PdfDocument outputPDFDocument = new PdfDocument();
      foreach (vw_DeliveryLine line in selectedLines)
      {
        String certificateLink = HttpContext.Current.Server.MapPath(line.CertificateLink);

        PdfDocument inputPDFDocument = CompatiblePdfReader.Open(certificateLink);
        outputPDFDocument.Version = inputPDFDocument.Version;
        foreach (PdfPage page in inputPDFDocument.Pages)
        {
          outputPDFDocument.AddPage(page);
        }
      }

      String databaseName = new DALPortalDataContext().Connection.Database;

      string filenameAndExtension = FilenameAndExtension(".pdf");
      string saveMergedPDFPath = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", filenameAndExtension));
      outputPDFDocument.Save(saveMergedPDFPath);
      string path = "../Files/Temp/" + filenameAndExtension;

      Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenPDFScript", "window.open(\"" + path + "\");", true);
    }



    private void DownloadFromPath(string path, string path2)
    {
      FileInfo fileInfo = new FileInfo(path2);
      if (fileInfo.Exists)
      {
        /*
        string filenameAndExtension = FilenameAndExtension(id, ".pdf");
        string saveMergedPDFPath = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", filenameAndExtension));
        string path = "../Files/Temp/" + filenameAndExtension;
        */

        Page.ClientScript.RegisterStartupScript(this.GetType(), "dlZip", "Download('" + path + "');", true);
        /*   Response.Clear();
           Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
           Response.AddHeader("Content-Length", fileInfo.Length.ToString());
           Response.ContentType = "application/octet-stream";
           Response.Flush();
           Response.TransmitFile(fileInfo.FullName);
           Response.End();*/
      }
    }

    private void CreateZipFile(List<vw_DeliveryLine> selectedLines)
    {
      String filenameAndExtension = FilenameAndExtension(".zip");
      String path = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", filenameAndExtension));
      String path2 = "../Files/Temp/" + filenameAndExtension;

      using (ZipArchive newFile = ZipFile.Open(path, ZipArchiveMode.Create))
      {
        foreach (vw_DeliveryLine line in selectedLines)
        {
          String certificateLink = HttpContext.Current.Server.MapPath(line.CertificateLink);
          newFile.CreateEntryFromFile(certificateLink, String.Format("{0}-{1}-{2}.pdf", line.DELDocNum, line.customerReference, line.HeatNumber));
        }
      }

      DownloadFromPath(path2, path);
    }

    protected void RadButtonSelectedInZip_Click(object sender, EventArgs e)
    {

      //List<String> certificateLinks = GetCertificateLinks();

      List<vw_DeliveryLine> selectedLines = GetSelectedVwLines();
      Int32 newCreditsUsed = 0;

      if (selectedLines.Count != 0)
      {
        int usedCreds = selectedLines.Count();
        double daysLeft = 0;

        Guid userId = GetUserId();
        contactPerson contactPerson = ContactPersonClass.GetContactPerson(userId, dc);
        businessPartner bpMother = new businessPartner();
        certificateBundle currentBundle = new certificateBundle() { modelId = 0 };
        
        if(contactPerson != null)
        {
          bpMother = BusinessPartnerClass.GetBusinessPartnerMother(userId, dc);
          currentBundle = CertificateBundleClass.GetCurrentBundle(bpMother.businessPartnerId, dc);
        }

        switch (currentBundle.modelId)
        {
          // Van Leeuwen User
          case 0:

            List<userRole> roles = dc.userRoles.Where(c => c.userId.Equals(userId)).ToList();
            Boolean siteAdmin = dc.userSettings.Any(c => c.siteAdmin && c.userId.Equals(userId));
            if (siteAdmin || roles.Any(c => c.roleCode.Equals("CERT_ACTMNG")) || roles.Any(c => c.roleCode.Equals("CERT_VLUSER")))
            {
              CreateZipFile(selectedLines);
            }
            else
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('No bundle active.');", true);
            }

            break;

          // One Year Access
          case 1:

            daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;
            if (daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
            }
            else
            {
              CreateZipFile(selectedLines);
              newCreditsUsed = CalculateCreditsUsed(selectedLines);
              RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
              UpdateCredits(newCreditsUsed, currentBundle);
            }

            break;

          // Certificate Credit x times 50
          case 2:

            newCreditsUsed = CalculateCreditsUsed(selectedLines);

            int certCount = currentBundle.actualCertQty.Value;
            daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;

            if (certCount < 1 && daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits and the bundle as expired.');", true);
            }
            else if (certCount < newCreditsUsed)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits.');", true);
            }
            else if (daysLeft < 0)
            {
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
            }
            else
            {
              CreateZipFile(selectedLines);
              RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
              UpdateCredits(newCreditsUsed, currentBundle);
            }

            break;

          default:

            CreateZipFile(selectedLines);
            newCreditsUsed = CalculateCreditsUsed(selectedLines);
            RegisterUsedCerts(selectedLines, contactPerson.contactPersonId, bpMother.businessPartnerId);
            UpdateCredits(newCreditsUsed, currentBundle);

            break;
        }

        UpdateModelInfo();
      }
      else
        LiteralNoCertificatesSelected.Visible = true;
    }

    private void RegisterUsedCerts(List<vw_DeliveryLine> selectedLines, Int32 contactPersonId, Int32 businessPartnerId)
    {
      //Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
      //contactPerson contactPerson = dc.contactPersons.Where(c => c.userId.Equals(userId)).SingleOrDefault();

      //if (contactPerson == null)
      //  return;

      //int bpId = contactPerson.businessPartnerId;

      //if (contactPerson.businessPartner.bpMother.HasValue)
      //  bpId = contactPerson.businessPartner.bpMother.Value;

      foreach (vw_DeliveryLine deliveryLine in selectedLines)
      {
        usageReport report = new usageReport();
        report.contactPersonId = contactPersonId;
        report.businessPartnerId = businessPartnerId;
        report.savedMark = DateTime.Now;
        report.companyCode =  deliveryLine.companyCode;
        report.baseLineNum = deliveryLine.DELLineNum;
        report.baseDocId = deliveryLine.DELDocId;
        report.baseDocType = "DL";
        report.batchId = deliveryLine.batchId;
        report.heatNumber = deliveryLine.HeatNumber;

        dc.usageReports.InsertOnSubmit(report);
      }

      dc.SubmitChanges();
    }

    //private Dictionary<String, String> GetCertificateLinks()
    //{
    //  Dictionary<String, String> certificateLinks = new Dictionary<String, String>();

    //  if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
    //    return certificateLinks;
    //  else
    //  {
    //    if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
    //    {
    //      IQueryable<vw_DeliveryLine> data = GetData();
    //      certificateLinks = data.Where(c => c.CertificateLink != null).Select(c => c.CertificateLink).Distinct().ToList();

    //      foreach(var certificate in certificateLinks)
    //      {
    //       // certificate.Key = HttpContext.Current.Server.MapPath(certificate.Key));
    //      }
    //    }
    //    else // some check boxes selected
    //    {
    //      GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
    //      foreach (GridDataItem data in gridItemCollection)
    //      {
    //        if (data.GetDataKeyValue("CertificateLink") != null)
    //          certificateLinks.Add(HttpContext.Current.Server.MapPath(data.GetDataKeyValue("CertificateLink").ToString()));
    //      }
    //    }

    //    //
    //    certificateLinks = GetExistingCertificateLinks(certificateLinks);
    //  }

    //  return certificateLinks;
    //}

    private List<String> GetSelectedHeats()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<string> certificateLinks = new List<string>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.HeatNumber = data.GetDataKeyValue("HeatNumber").ToString();
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingHeats(certificates);
      }

      return certificateLinks;
    }

    private List<String> GetSelectedCompanyCode()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<string> certificateLinks = new List<string>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              // line1.CardCode = data.GetDataKeyValue("CompanyCode").ToString();
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingComCodes(certificates);
      }

      return certificateLinks;
    }

    private List<String> GetSelectedDocId()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<string> certificateLinks = new List<string>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.SODocNum = data.GetDataKeyValue("SODocNum").ToString();
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingDociD(certificates);
      }

      return certificateLinks;
    }

    private List<int> GetSelectedLineNum()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<int> certificateLinks = new List<int>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.SOLineNum = Convert.ToInt32(data.GetDataKeyValue("SOLineNum"));
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        //certificateLinks = GetExistingLineNum(certificates);
      }

      return certificateLinks;
    }

    private List<int> GetSelectedBatchId()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<int> certificateLinks = new List<int>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.batchId = Convert.ToInt32(data.GetDataKeyValue("batchId"));
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingBatchId(certificates);
      }

      return certificateLinks;
    }

    protected void RadGridCertificate_ItemCreated(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridHeaderItem)
      {
        GridHeaderItem headerItem = (GridHeaderItem)e.Item;
        CheckBox checkBox = (CheckBox)headerItem["ClientSelectColumn"].Controls[0];
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


    private List<string> GetSelectedDelDocNum()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<string> certificateLinks = new List<string>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.DELDocNum = data.GetDataKeyValue("DELDocNum").ToString();
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingDelDocNum(certificates);
      }

      return certificateLinks;
    }

    private List<vw_DeliveryLine> GetSelectedVwLines()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<vw_DeliveryLine> certificateLinks = new List<vw_DeliveryLine>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();

          // Check if files exists
          certificateLinks = certificates.Where(c => File.Exists(HttpContext.Current.Server.MapPath(c.CertificateLink))).ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.companyCode = data.GetDataKeyValue("companyCode").ToString();
              line1.DELDocId = Convert.ToInt32(data.GetDataKeyValue("DELDocId"));
              line1.DELDocNum = data.GetDataKeyValue("DELDocNum").ToString();
              line1.DELLineNum = Convert.ToInt32(data.GetDataKeyValue("DELLineNum").ToString());
              line1.batchId = Convert.ToInt32(data.GetDataKeyValue("batchId"));
              line1.HeatNumber = data.GetDataKeyValue("HeatNumber").ToString();
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              line1.customerReference = data.GetDataKeyValue("CustomerReference").ToString();

              string filePath = HttpContext.Current.Server.MapPath(line1.CertificateLink);
              if (File.Exists(filePath))
                certificateLinks.Add(line1);
            }
          }
        }
      }

      return certificateLinks;
    }

    private List<int> GetSelectedDELLineNum()
    {
      List<vw_DeliveryLine> certificates = new List<vw_DeliveryLine>();
      List<int> certificateLinks = new List<int>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_DeliveryLine> data = GetData();
          certificates = data.Where(c => c.CertificateLink != null).Select(c => c).Distinct().ToList();
        }

        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("CertificateLink") != null)
            {
              vw_DeliveryLine line1 = new vw_DeliveryLine();
              line1.DELLineNum = (int)data.GetDataKeyValue("DELLineNum");
              line1.CertificateLink = data.GetDataKeyValue("CertificateLink").ToString();
              certificates.Add(line1);
            }
            // certificateLinks.Add(data.GetDataKeyValue("HeatNumber").ToString());

          }
        }

        //
        certificateLinks = GetExistingDelLineNum(certificates);
      }

      return certificateLinks;
    }

    protected void RadGridCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
      if (e.CommandName == "Outsource")
      {
        int rowindex = e.Item.ItemIndex;
        GridDataItem item = (GridDataItem)e.Item;

        string certificateLink = item["CertificateLink"].Text.ToString();
        string heatNumber = item["HeatNumber"].Text.ToString();
        string companyCode = item["CardCode"].Text.ToString();
        string delLineNum = item["DELLineNum"].Text.ToString();
        string delDocNum = item["DELDocNum"].Text.ToString();
        string batchIdString = item["batchId"].Text.ToString();
        string customerReference = item["CustomerReference"].Text.ToString();

        string url = ResolveUrl(certificateLink);
        string filePath = HttpContext.Current.Server.MapPath(certificateLink);
        double daysLeft = 0;

        if (!File.Exists(filePath))
        {
          ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('This file does not exist.');", true);
          return;
        }

        // Copy file to Temp folder and rename
        String newFileName = String.Format("{0}-{1}-{2}.pdf", delDocNum, customerReference, heatNumber);
        String tempPath = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", newFileName));
        File.Copy(filePath, tempPath, true);
        String tempUrl = ResolveUrl("../Files/Temp/" + newFileName);


        Guid userId = GetUserId();
        contactPerson contactPerson = ContactPersonClass.GetContactPerson(userId, dc);
        businessPartner bpMother = new businessPartner();
        certificateBundle currentBundle = new certificateBundle() { modelId = 0 };

        if (contactPerson != null)
        {
          bpMother = BusinessPartnerClass.GetBusinessPartnerMother(userId, dc);
          currentBundle = CertificateBundleClass.GetCurrentBundle(bpMother.businessPartnerId, dc);
        }

        int batchId = Convert.ToInt32(batchIdString);

        vw_DeliveryLine delLine = dc.vw_DeliveryLines.Where(c => c.batchId.Equals(batchId) && c.DELDocNum.Equals(delDocNum) && c.DELLineNum.Equals(delLineNum)).Select(c => c).SingleOrDefault();
        List<usageReport> reports = dc.usageReports.Where(c => c.batchId.Equals(batchId) && c.baseDocId.Equals(delLine.DELDocId) && c.baseLineNum.Equals(delLine.DELLineNum)).Select(c => c).ToList();

        int newCreditsUsed = 0;

        if (contactPerson != null)
        {
          if (reports.Count == 0)
          {
            newCreditsUsed = 1;
            usageReport report = new usageReport();

            report.heatNumber = heatNumber;
            report.contactPersonId = contactPerson.contactPersonId;
            report.businessPartnerId = bpMother.businessPartnerId;
            report.savedMark = DateTime.Now;
            report.companyCode = delLine.companyCode;
            report.batchId = batchId;//
            report.baseLineNum = delLine.DELLineNum;
            report.baseDocId = delLine.DELDocId;
            report.baseDocType = delLine.DELDocType;

            dc.usageReports.InsertOnSubmit(report);
            dc.SubmitChanges();
          }


          switch (currentBundle.modelId)
          {
            case 0:

              List<userRole> roles = dc.userRoles.Where(c => c.userId.Equals(userId)).ToList();
              if (roles.Any(c => c.roleCode.Equals("CERT_ACTMNG")) || roles.Any(c => c.roleCode.Equals("CERT_VLUSER")))
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);
              else
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('No bundle active.');", true);

              break;

            case 1:

              daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;

              if (daysLeft < 0)
              {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
              }
              else
              {
                UpdateCredits(newCreditsUsed, currentBundle);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);
              }
              break;

            case 2:

              int certCount = currentBundle.actualCertQty.Value;
              daysLeft = (currentBundle.expireDate.Value - DateTime.Now).TotalDays;

              if (certCount < 1 && daysLeft < 0)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits and the bundle has expired.');", true);
              else if (certCount < 1)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('Insufficient credits.');", true);
              else if (daysLeft < 0)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myalert", "alert('The bundle has expired.');", true);
              else
              {
                UpdateCredits(newCreditsUsed, currentBundle);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);
              }

              break;

            case 4:

              UpdateCredits(newCreditsUsed, currentBundle);
              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);

              break;

            default:

              ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);
              break;
          }
        }
        else
        {
          UpdateCredits(newCreditsUsed, currentBundle);
          ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + tempUrl + "','_blank');", true);
        }

        UpdateModelInfo();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "registerNewCred", "setContentHeader()", true);
      }
    }

    private static Guid GetUserId()
    {
      return (Guid)Membership.GetUser().ProviderUserKey;
    }

    private bool RemoteFileExists(string url)
    {
      try
      {
        //Creating the HttpWebRequest
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //Setting the Request method HEAD, you can also use GET too.
        request.Method = "HEAD";
        //Getting the Web Response.
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //Returns TRUE if the Status code == 200
        return (response.StatusCode == HttpStatusCode.OK);
      }
      catch
      {
        //Any exception will returns false.
        return false;
      }
    }

    protected void usageReportBut_Click(object sender, EventArgs e)
    {
      string bpMotherId = labelBpId.Text;
      string url = ResolveUrl("UsageReport.aspx?id=" + bpMotherId);
      ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + url + "','_blank');", true);

    }
  }
}
