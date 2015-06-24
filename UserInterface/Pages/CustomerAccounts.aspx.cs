using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class CustomerAccounts : BasePage
	{
		private string bpName = null;
		private string bpCode = null;
		private string contactPerson = null;
		private static DALPortalDataContext dc;
		protected void Page_Load(object sender, EventArgs e)
		{
			dc = new DALPortalDataContext();
			if (IsPostBack)
			{
				if (ViewState["bpName"] != null)
					bpName = ViewState["bpName"].ToString();
				if (ViewState["bpCode"] != null)
					bpCode = ViewState["bpCode"].ToString();
			}
		}

		protected void RadGridCertificate_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
		{
			if (e.CommandName == "Outsource")
			{
				int rowindex = e.Item.ItemIndex;
				GridDataItem item = (GridDataItem)e.Item;

				Int32 bpId = Convert.ToInt32(item["businessPartnerId"].Text);
				Session["businessPartnerId"] = bpId;
				Session["bpCode"] = item["bpCode"].Text.ToString();
				Response.Redirect("~/Pages/CustomerAccountDetails.aspx");
			}

		}

		protected void autoCompleteBox_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			var data = GetData();
			var result = data.Select(c => new { c.bpName }).Distinct();

			e.Result = result;
		}

		private IQueryable<businessPartner> GetData()
		{
			var predicate = PredicateExtensions.True<businessPartner>();


			if (!String.IsNullOrEmpty(bpName))
				predicate = predicate.And(c => c.bpName.Equals(bpName));
			if (!String.IsNullOrEmpty(bpCode))
				predicate = predicate.And(c => c.bpCode.Equals(bpCode));

			return dc.businessPartners.Where(predicate).OrderByDescending(c => c.bpName);
		}

		protected void RadAutoCompleteBoxBpName_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
		{
			AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
			if (entries.Count != 0)
			{
				this.bpName = entries[0].Text;
				ViewState["bpName"] = entries[0].Text;
			}
			else
			{
				this.bpName = null;
				ViewState["bpName"] = null;
			}

			RefreshRadGridCertificate();
		}

		private void RefreshRadGridCertificate()
		{
			// Kunnen weg?
			RadGrid1.MasterTableView.SortExpressions.Clear();
			RadGrid1.MasterTableView.GroupByExpressions.Clear();
			RadGrid1.MasterTableView.CurrentPageIndex = 0;

			// Moet blijven
			RadGrid1.Rebind();
		}

		protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			e.Result = GetData();
		}

		protected void RadAutoCompleteBoxCompanyCode_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
		{
			AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
			if (entries.Count != 0)
			{
				this.bpCode = entries[0].Text;
				ViewState["bpCode"] = entries[0].Text;
			}
			else
			{
				this.bpCode = null;
				ViewState["bpCode"] = null;
			}

			RefreshRadGridCertificate();
		}

		protected void autoCompleteBoxBpCode_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			var data = GetData();
			var result = data.Select(c => new { c.bpCode }).Distinct();

			e.Result = result;
		}
	}
}