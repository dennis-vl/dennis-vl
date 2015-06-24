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
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages.DOP
{
  public partial class DOPDetails : BasePage
  {
    private static DALPortalDataContext dc;
    private bool radGridCheckBoxSellectAllChecked = false;
    protected void Page_Load(object sender, EventArgs e)
    {

      dc = new DALPortalDataContext();
      if (IsPostBack)
      {
        if (ViewState["radGridCheckBoxSellectAllChecked"] != null)
          radGridCheckBoxSellectAllChecked = (bool)ViewState["radGridCheckBoxSellectAllChecked"];
      }

      //  RadGridCertificate.DataSource = null;
      //  RadGridCertificate.DataBind();
      if (!IsPostBack)
      {
        RadGridCertificate.Visible = false;
        RadButtonMergePDFs.Visible = false;
        RadButtonSelectedInZip.Visible = false;
      }
      Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
      int bpId = dc.contactPersons.Where(c => c.userId.Equals(userId)).Select(c => c.businessPartnerId).SingleOrDefault();
      var isSister = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c.bpMother).SingleOrDefault();
      var hasRole = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(bpId)).Where(c => c.applicationCode.Equals("DOP")).Select(c => c).SingleOrDefault();
      if (!UserIsAdmin())
      {
        if (hasRole == null)
        {
          if (isSister > 0)
          {
            var hasRole2 = dc.businessPartnerApplications.Where(c => c.businessPartnerId.Equals(isSister)).Where(c => c.applicationCode.Equals("DOP")).Select(c => c).SingleOrDefault();
            if (hasRole2 == null)
            {
              SearchTxtBox.Enabled = false;
              SearchButton.Enabled = false;
              warningLiteral.Visible = true;
              warningLiteral.Text = "Your account has been disabled, please contact your accountmanager.";
            }
          }
          else
          {
            SearchTxtBox.Enabled = false;
            SearchButton.Enabled = false;
            warningLiteral.Visible = true;
            warningLiteral.Text = "Your account has been disabled, please contact your accountmanager.";
          }
        }
      }

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

    protected void searchButton_Click(object sender, EventArgs e)
    {
      RadGridCertificate.Visible = true;
      RadButtonMergePDFs.Visible = true;
      RadButtonSelectedInZip.Visible = true;
      // gridButtonsDiv.Visible = true;
      // IQueryable<vw_businessPartnerDocument> documents = dc.vw_businessPartnerDocuments.Where(c => c.bpName.Equals(txtFilled));
      //businessPartnerDocumentsDataSource.WhereParameters.Add("bpName" , txtFilled);
      //businessPartnerDocumentsDataSource.DataBind();
      //RadGridCertificate.DataBind();

      RadGridCertificate.MasterTableView.SortExpressions.Clear();
      RadGridCertificate.MasterTableView.GroupByExpressions.Clear();
      RadGridCertificate.MasterTableView.CurrentPageIndex = 0;
      // RadGridCertificate.DataSource = null;
      // Moet blijven
      RadGridCertificate.Rebind();

      //   RadGridCertificate.DataBind();
      //  businessPartnerDocumentsDataSource.WhereParameters.Clear();
      // RadGridCertificate.Rebind();
      // RadGridCertificate.DataSource = businessPartnerDocumentsDataSource;
      //  RadGridCertificate.Rebind();
      // RadGridCertificate.DataBinding(documents);

    }

    protected void businessPartnerDocumentsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      //string txtFilled = searchTxtBox.Text;
      //businessPartnerDocumentsDataSource.WhereParameters.Clear();

      //int result = 0;
      //if (dc.businessPartnerSearches.Any(b => b.searchString.Equals(txtFilled)))
      //{
      //    result = dc.businessPartnerSearches.Where(b => b.searchString.Equals("ACI Industries")).SingleOrDefault().businessPartnerId;
      //}else
      //{
      //    businessPartnerDocumentsDataSource.WhereParameters.Add("bpName", null);
      //}



      // businessPartnerDocumentsDataSource.WhereParameters.Add("businessPartnerId", result.ToString());

      e.Result = GetData();
    }

    protected void businessPartnerDocumentsDataSource_Selected(object sender, LinqDataSourceStatusEventArgs e)
    {
      string test = " tes";
    }

    protected void RadButtonSelectedInZip_Click(object sender, EventArgs e)
    {
      List<String> certificateLinks = GetCertificateLinks();

      if (certificateLinks.Count != 0)
      {
        String databaseName = new DALPortalDataContext().Connection.Database;

        string filenameAndExtension = FilenameAndExtension(".zip");
        // string path = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", filenameAndExtension));
        string path = HttpContext.Current.Server.MapPath(Path.Combine(@"~\Files\Temp", filenameAndExtension));
        using (ZipArchive newFile = ZipFile.Open(path, ZipArchiveMode.Create))
        {
          foreach (string certificateLink in certificateLinks)
          {
            newFile.CreateEntryFromFile(certificateLink, Path.GetFileName(certificateLink));
          }
        }

        downloadFromPath(path);
      }
      else
        LiteralNoCertificatesSelected.Visible = true;
    }


    private List<String> GetCertificateLinks()
    {
      List<String> certificateLinks = new List<String>();

      if (RadGridCertificate.SelectedItems.Count == 0) // no check boxes selected
        return certificateLinks;
      else
      {
        if (radGridCheckBoxSellectAllChecked) // sellect all check box selected
        {
          IQueryable<vw_businessPartnerDocument> data = GetData();
          certificateLinks = data.Where(c => c.DOPLink != null).Select(c => c.DOPLink).Distinct().ToList();

          for (int i = 0; i < certificateLinks.Count; i++)
            certificateLinks[i] = HttpContext.Current.Server.MapPath(certificateLinks[i]);
        }
        else // some check boxes selected
        {
          GridItemCollection gridItemCollection = RadGridCertificate.SelectedItems;
          foreach (GridDataItem data in gridItemCollection)
          {
            if (data.GetDataKeyValue("DOPLink") != null)
              certificateLinks.Add(HttpContext.Current.Server.MapPath(data.GetDataKeyValue("DOPLink").ToString()));
          }
        }

        //
        certificateLinks = GetExistingCertificateLinks(certificateLinks);
      }

      return certificateLinks;
    }

    private List<string> GetExistingCertificateLinks(List<string> certificateLinks)
    {
      List<string> existingCertificates = new List<string>();
      foreach (string certificateLink in certificateLinks)
      {
        if (File.Exists(certificateLink))
          existingCertificates.Add(certificateLink);
      }

      return existingCertificates;
    }

    private IQueryable<vw_businessPartnerDocument> GetData()
    {
      List<Int32> bpIds = dc.businessPartnerSearches.Where(c => c.searchString.Equals(SearchTxtBox.Text)).Select(c => c.businessPartnerId).ToList();

      var predicate = PredicateExtensions.True<vw_businessPartnerDocument>();

      if (bpIds.Count > 0)
        predicate = predicate.And(c => bpIds.Contains(c.businessPartnerId));
      else
        predicate = predicate.And(c => c.businessPartnerId.Equals(-1));

      return dc.vw_businessPartnerDocuments.Where(predicate).OrderByDescending(c => c.bpName).ThenBy(c => c.description);
      //return dc.vw_DeliveryLines.Where(predicate).OrderByDescending(c => c.DELDocDate).ThenByDescending(d => d.DELDocNum).ThenBy(d => d.DELLineNum);
    }

    private static string FilenameAndExtension(string extension)
    {
      string timeCreated = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year +
        " " + DateTime.Now.Hour + " " + DateTime.Now.Minute + " " + DateTime.Now.Second;
      return timeCreated + extension;
    }

    private void downloadFromPath(string path)
    {
      // update setting with filepath.
      Session["FileToDownload"] = path;

      // redirect user by using an iframe to Download.aspx. Because download doesn't work from within an Update Panel
      String script = "var iframe = document.createElement('iframe'); "
      + "iframe.src = '/Pages/Download.aspx'; "	// Point the IFRAME to GenerateFile, with the desired region as a querystring argument.
      + "iframe.style.display = 'none'; "				// This makes the IFRAME invisible to the user.
      + "document.body.appendChild(iframe); ";	// Add the IFRAME to the page.  This will trigger a request to GenerateFile now.

      ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenPDFScript", script, true);

      //FileInfo fileInfo = new FileInfo(path);
      //if (fileInfo.Exists)
      //{
      //		Response.Clear();
      //		Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
      //		Response.AddHeader("Content-Length", fileInfo.Length.ToString());
      //		Response.ContentType = "application/octet-stream";
      //		Response.Flush();
      //		Response.TransmitFile(fileInfo.FullName);
      //		Response.End();
      //}
    }

    protected void RadButtonMergePDFs_Click(object sender, EventArgs e)
    {
      List<String> certificateLinks = GetCertificateLinks();

      if (certificateLinks.Count != 0)
      {
        PdfDocument outputPDFDocument = new PdfDocument();
        foreach (string certificateLink in certificateLinks)
        {
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenPDFScript", "window.open(\"../" + path + "\");", true);

        //downloadFromPath(path);
      }
      else
        LiteralNoCertificatesSelected.Visible = true;
    }

    protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {

    }

    protected void RadGridCertificate_ItemCreated(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridHeaderItem)
      {
        GridHeaderItem headerItem = (GridHeaderItem)e.Item;
        CheckBox checkBox = (CheckBox)headerItem["ClientSelectionColumn"].Controls[0];
        checkBox.AutoPostBack = true;
        checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
      }
    }

    private void checkBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox)sender;

      if (checkBox.Checked)
        ViewState["radGridCheckBoxSellectAllChecked"] = true;
      else
        ViewState["radGridCheckBoxSellectAllChecked"] = false;
    }
  }
}