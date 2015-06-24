using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class Download : System.Web.UI.Page
	{
		/// <summary>
		/// Page to let users download files. Used when the download button on the page is in an Update Panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			String path = Convert.ToString(Session["FileToDownload"]);
			
			FileInfo fileInfo = new FileInfo(path);
			if (fileInfo.Exists)
			{
				Response.Clear();
				Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
				Response.AddHeader("Content-Length", fileInfo.Length.ToString());
				Response.ContentType = "application/octet-stream";
				Response.Flush();
				Response.TransmitFile(fileInfo.FullName);
				Response.End();
			}
		}
	}
}