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

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class UserAccounts : BasePage
	{
		private static DALPortalDataContext dc;
		private string name = null;
		private string userName = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			dc = new DALPortalDataContext();
			if (IsPostBack)
			{
				if (ViewState["name"] != null)
					name = ViewState["name"].ToString();
				if (ViewState["UserName"] != null)
					userName = ViewState["UserName"].ToString();
			}
		}

		protected void createAccount_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Pages/CreateUser.aspx");
		}

		protected void RadGridCertificate_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
		{
			if (e.CommandName == "Outsource")
			{
				int rowindex = e.Item.ItemIndex;
				GridDataItem item = (GridDataItem)e.Item;
				string strTxt = item["name"].Text.ToString();
				Session["name"] = strTxt;
				Session["username"] = item["UserName"].Text.ToString();
				Response.Redirect("~/Pages/AddRole.aspx");
			}

		}



		protected void RadAutoCompleteBoxName_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
		{
			AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
			if (entries.Count != 0)
			{
				this.name = entries[0].Text;
				ViewState["name"] = entries[0].Text;
			}
			else
			{
				this.name = null;
				ViewState["name"] = null;
			}

			RefreshRadGrid();
		}

		private void RefreshRadGrid()
		{
			RadGridCertificate.MasterTableView.SortExpressions.Clear();
			RadGridCertificate.MasterTableView.GroupByExpressions.Clear();
			RadGridCertificate.MasterTableView.CurrentPageIndex = 0;

			RadGridCertificate.Rebind();
		}

		private IQueryable<vw_name_in_user> GetData()
		{
			var predicate = PredicateExtensions.True<vw_name_in_user>();

			if (!String.IsNullOrEmpty(name))
				predicate = predicate.And(c => c.name.Equals(name));
			if (!String.IsNullOrEmpty(userName))
				predicate = predicate.And(c => c.UserName.Equals(userName));

			return dc.vw_name_in_users.Where(predicate).OrderByDescending(c => c.name);
		}


		protected void AutoCompleteBoxNameDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			var data = GetData();
			var result = data.Select(c => new { c.name }).Distinct();

			e.Result = result;
		}

		protected void DataSourceLinq_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			e.Result = GetData();
		}

		protected void AutoCompleteBoxUserNameDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			var data = GetData();
			var result = data.Select(c => new { c.UserName }).Distinct();

			e.Result = result;
		}

		protected void RadAutoCompleteBoxUserName_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
		{
			AutoCompleteBoxEntryCollection entries = ((RadAutoCompleteBox)sender).Entries;
			if (entries.Count != 0)
			{
				this.userName = entries[0].Text;
				ViewState["UserName"] = entries[0].Text;
			}
			else
			{
				this.userName = null;
				ViewState["UserName"] = null;
			}

			RefreshRadGrid();
		}
	}
}