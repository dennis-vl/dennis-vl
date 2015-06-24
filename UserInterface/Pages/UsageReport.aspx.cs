using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using Telerik.Web.UI.GridExcelBuilder;
using xi = Telerik.Web.UI.ExportInfrastructure;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class UseageReport : BasePage
	{
		string id;
		string headerMiddleCell = "<h2>Usage report</h2>";
		string footerMiddleCell = "<?page-number?>";
		protected void Page_Load(object sender, EventArgs e)
		{
			id = Request.QueryString["id"];

			if (!IsPostBack)
			{
				RadGrid1.ExportSettings.Pdf.BorderType = GridPdfSettings.GridPdfBorderType.AllBorders;
				RadGrid1.ExportSettings.Pdf.PageHeader.MiddleCell.Text = headerMiddleCell;
				RadGrid1.ExportSettings.Pdf.PageHeader.MiddleCell.TextAlign = GridPdfPageHeaderFooterCell.CellTextAlign.Center;
				RadGrid1.ExportSettings.Pdf.PageFooter.MiddleCell.Text = footerMiddleCell;
				RadGrid1.ExportSettings.Pdf.PageFooter.MiddleCell.TextAlign = GridPdfPageHeaderFooterCell.CellTextAlign.Center;
				RadGrid1.ExportSettings.Pdf.ContentFilter = GridPdfFilter.NoFilter;
			}

			// Session["businessPartnerId"] = id;
		}

		protected void FormatGridItem(GridItem item)
		{
			item.Style["color"] = "#000";

			if (item is GridDataItem)
			{
				item.Style["vertical-align"] = "middle";
				item.Style["text-align"] = "center";
			}

			switch (item.ItemType) //Mimic RadGrid appearance for the exported PDF file
			{
				/* case GridItemType.Item: item.Style["background-color"] = "#4F4F4F"; break;

				 case GridItemType.AlternatingItem: item.Style["background-color"] = "#494949"; break;

				 case GridItemType.Header: item.Style["background-color"] = "#2B2B2B"; break;

				 case GridItemType.CommandItem: item.Style["background-color"] = "#000000"; break;
				 */
			}

			if (item is GridCommandItem)
			{
				item.PrepareItemStyle();  //needed to span the image over the CommandItem cells
			}
		}

		private static string FilenameAndExtension(string id, string extension)
		{
			string timeCreated = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year +
				" " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second;
			return id + " " + timeCreated + extension;
		}

		protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
		{
			if (isPdfExport)

				FormatGridItem(e.Item);
		}

		bool isPdfExport = false;
		protected void PrintOnScreen_Click(object sender, EventArgs e)
		{
      RadGrid1.ExportSettings.FileName = "Usage report " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year +
         " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second;
			RadGrid1.MasterTableView.ExportToPdf();
		}

		protected void ExportExcel_Click(object sender, EventArgs e)
		{
			string alternateText = "Xlsx";
			#region [ XSLX FORMAT ]
			if (alternateText == "Xlsx")
			{
				RadGrid1.MasterTableView.GetColumn("documentNumber").HeaderStyle.BackColor = Color.LightGray;
				RadGrid1.MasterTableView.GetColumn("documentNumber").ItemStyle.BackColor = Color.LightGray;
			}
			#endregion
			RadGrid1.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), alternateText);
      RadGrid1.ExportSettings.FileName = "Usage report " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year +
				" " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second;
      RadGrid1.ExportSettings.IgnorePaging = false;
			RadGrid1.ExportSettings.ExportOnlyData = true;
			RadGrid1.ExportSettings.OpenInNewWindow = true;
			RadGrid1.MasterTableView.ExportToExcel();
		}
	}
}