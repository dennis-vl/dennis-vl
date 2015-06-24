using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Framework.Extensions;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using VanLeeuwen.Framework;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class SelectSisters : BasePage
	{
		private int businessPartnerId;
		private DALPortalDataContext dc;
		private string bpCode;

		protected void Page_Load(object sender, EventArgs e)
		{
			businessPartnerId = (Int32)Session["businessPartnerId"];
			dc = new DALPortalDataContext();
			if (IsPostBack)
			{
				if (ViewState["bpCode"] != null)
					bpCode = ViewState["bpCode"].ToString();
			}
		}


		protected void submitButton_Click(object sender, EventArgs e)
		{
			// Clear all sisters
			List<Int32> businessPartners = dc.businessPartners.Where(c => c.bpMother.Equals(businessPartnerId)).Select(c => c.businessPartnerId).ToList();
			foreach (Int32 bpId in businessPartners)
			{
				businessPartner bp = dc.businessPartners.Where(c => c.businessPartnerId.Equals(bpId)).Select(c => c).SingleOrDefault();
				bp.bpMother = null;
			}

      // Add sisters
      List<String> selectedListItems = RadListBoxDestination.Items.Select(c => c.Value).ToList();
			foreach (String selectedBp in selectedListItems)
			{
        businessPartner bp = dc.businessPartners.Where(c => c.businessPartnerId.Equals(Convert.ToInt32(selectedBp))).Select(c => c).SingleOrDefault();
				bp.bpMother = businessPartnerId;
			}

			dc.SubmitChanges();

			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/CustomerAccountDetails.aspx");
		}

		protected void cancelButton_Click(object sender, EventArgs e)
		{

			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/CustomerAccountDetails.aspx");
		}

		private IQueryable<businessPartner> GetData()
		{
			var predicate = PredicateExtensions.True<businessPartner>();

			if (!String.IsNullOrEmpty(bpCode))
				predicate = predicate.And(c => c.bpCode.Equals(bpCode));

			predicate = predicate.And(c => c.bpMother.Equals(null));
      predicate = predicate.And(c => c.bpType.Equals("C"));
      predicate = predicate.And(c => !c.isMother);

			return dc.businessPartners.Where(predicate).OrderBy(c => Convert.ToInt32(c.bpCode));
		}

		protected void RadAutoCompleteBoxBpName_EntryAdded(object sender, Telerik.Web.UI.AutoCompleteEntryEventArgs e)
		{
			AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
			if (entries.Count != 0)
			{
        String selectedText = entries[0].Text;
        bpCode = StringFunctions.GetPartOfString(selectedText, " - ", 1);
				ViewState["bpCode"] = entries[0].Text;
			}
			else
			{
				bpCode = null;
				ViewState["bpCode"] = null;
			}

      RadListBoxSource.DataBind();
		}

		protected void autoCompleteBoxBpName_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
      e.Result = GetData().Select(c => new { c.businessPartnerId, bpName = c.bpCode + " - " + c.bpName });
		}

    protected void LinqDataSourceSrc_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      e.Result = GetData().Select(c => new { c.businessPartnerId, bpName = c.bpCode + " - " + c.bpName});
    }

    protected void LinqDataSourceDest_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
      e.Result = dc.businessPartners.Where(c => c.bpMother.Equals(businessPartnerId) && !c.isMother).OrderBy(c => Convert.ToInt32(c.bpCode)).Select(c => new { c.businessPartnerId, bpName = c.bpCode + " - " + c.bpName });
    }
	}
}