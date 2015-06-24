using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Pages
{
	public partial class CertficateModelDetails : BasePage
	{
		private Int32 businessPartnerId;
		private Int32 changed;
		private Int32 modelId;
		protected void Page_Load(object sender, EventArgs e)
		{
			businessPartnerId = (Int32)Session["businessPartnerId"];
			labelBpId.Text = businessPartnerId.ToString();

			if (!IsPostBack)
			{
				validUntilDTP.SelectedDate = new DateTime(2017, 10, 27);
				changed = 0;
				modelId = 0;

				getData();
			}

			var state1 = btnModel1.SelectedToggleState.Selected;
			var state2 = btnModel2.SelectedToggleState.Selected;
			var state3 = btnModel3.SelectedToggleState.Selected;
			var state4 = btnModel4.SelectedToggleState.Selected;
			var state5 = btnModel5.SelectedToggleState.Selected;
			if (state1)
			{
				state1Selected();
			}
			if (state2)
			{
				state2Selected();
			}
			if (state3)
			{
				state3Selected();
			}
			if (state4)
			{
				state4Selected();
			}
			if (state5)
			{
				state5Selected();
			}

		}

		private void getData()
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			certificateBundle bundle = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(businessPartnerId) && c.isActive.Equals(true)).SingleOrDefault();
			if (bundle != null)
			{
				creditsPurchasedTxtBox.Value = bundle.orderedCertQty;
				creditsUsedTxtBox.Value = bundle.usedCertQty;
				creditsLeftTxtBox.Value = bundle.actualCertQty;
				validUntilDTP.SelectedDate = bundle.expireDate;
				if (bundle.certificatePrice == null)
				{
					priceTextBox.Value = 7.50;
				}
				else
				{
					priceTextBox.Value = (Double)bundle.certificatePrice;
				}

				switch (bundle.modelId)
				{ 
					case 1:
						btnModel1.Checked = true;
						break;
					case 2:
						btnModel2.Checked = true;
						break;
					case 3:
						btnModel3.Checked = true;
						break;
					case 4:
						btnModel4.Checked = true;
						break;
					case 5:
						btnModel5.Checked = true;
						break;
				}

				this.modelId = bundle.modelId;
			}
		}

		private void state1Selected()
		{
			showAll();
			creditsLeftTxtBox.Enabled = false;
			priceTextBox.Enabled = false;
			this.modelId = 1;
			//int changed = 0;
			if (changed == 0)
			{
				DateTime dateNow = DateTime.Now.AddYears(1);
				validUntilDTP.SelectedDate = dateNow;
				this.changed = 1;
			}

			updradeButton.Enabled = false;
		}

		private void state2Selected()
		{
			showAll();
			this.modelId = 2;
			if (changed == 0)
			{
				DateTime dateNow = DateTime.Now.AddYears(1);
				validUntilDTP.SelectedDate = dateNow;
				this.changed = 1;
			}
			updradeButton.Enabled = true;
		}

		private void state3Selected()
		{
			showAll();
			this.modelId = 3;
			creditsLeftTxtBox.Enabled = false;
			priceTextBox.Enabled = false;
			validUntilDTP.Enabled = false;
			updradeButton.Enabled = false;
		}

		private void state4Selected()
		{
			showAll();
			this.modelId = 4;
			creditsLeftTxtBox.Enabled = false;
			//   priceTextBox.Enabled = false;
			validUntilDTP.Enabled = false;
			updradeButton.Enabled = false;
		}

		private void state5Selected()
		{
			showAll();
			this.modelId = 5;
			creditsLeftTxtBox.Enabled = false;
			priceTextBox.Enabled = false;
			validUntilDTP.Enabled = false;
			updradeButton.Enabled = false;
		}

		private void showAll()
		{
			priceTextBox.Enabled = true;
			validUntilDTP.Enabled = true;
			creditsLeftTxtBox.Enabled = true;
			updradeButton.Enabled = true;
		}



		protected void RadCancelButton_Click(object sender, EventArgs e)
		{
			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/CustomerAccountDetails.aspx");
		}

		protected void RadSaveButton_Click(object sender, EventArgs e)
		{
			DALPortalDataContext dc = new DALPortalDataContext();

			certificateBundle bundleOld = dc.certificateBundles.Where(c => c.businessPartnerId.Equals(businessPartnerId) && c.isActive.Equals(true)).SingleOrDefault();

			if (bundleOld != null)
			{
				if (this.modelId == bundleOld.modelId)
				{
					bundleOld.startDate = DateTime.Now;
					bundleOld.expireDate = validUntilDTP.SelectedDate;
					bundleOld.previousCertQty = bundleOld.actualCertQty;
					// bundle.orderedCertQty = (Int32)creditsLeftTxtBox.Value - bundle.previousCertQty;
					bundleOld.actualCertQty = (Int32)creditsLeftTxtBox.Value;
					bundleOld.certificatePrice = (Decimal)priceTextBox.Value;
					bundleOld.isActive = true;
					// bundleOud.modelId = modelId;
					bundleOld.companyCode = "ZW";
					bundleOld.businessPartnerId = businessPartnerId;
				}
				else
				{
					bundleOld.isActive = false;
					certificateBundle bundelNew = new certificateBundle();
					bundelNew.startDate = DateTime.Now;
					bundelNew.expireDate = validUntilDTP.SelectedDate;
					bundelNew.previousCertQty = bundleOld.actualCertQty;
					bundelNew.orderedCertQty = 0;
					bundelNew.actualCertQty = (Int32)creditsLeftTxtBox.Value;
					bundelNew.certificatePrice = (Decimal)priceTextBox.Value;
					bundelNew.isActive = true;
					bundelNew.modelId = modelId;
					bundelNew.companyCode = "ZW";
					bundelNew.businessPartnerId = businessPartnerId;
					dc.certificateBundles.InsertOnSubmit(bundelNew);
				}
			}
			else
			{
				certificateBundle bundelNew = new certificateBundle();
				bundelNew.startDate = DateTime.Now;
				bundelNew.expireDate = validUntilDTP.SelectedDate;
				bundelNew.previousCertQty = 0;
				bundelNew.orderedCertQty = 0;
				bundelNew.actualCertQty = (Int32)creditsLeftTxtBox.Value;
				bundelNew.certificatePrice = (Decimal)priceTextBox.Value;
				bundelNew.isActive = true;
				bundelNew.modelId = modelId;
				bundelNew.companyCode = "ZW";
				bundelNew.businessPartnerId = businessPartnerId;
				dc.certificateBundles.InsertOnSubmit(bundelNew);
			}

			dc.SubmitChanges();

			Session["businessPartnerId"] = businessPartnerId;
			Response.Redirect("~/Pages/CustomerAccountDetails.aspx");
		}

		protected void usageReport_Click(object sender, EventArgs e)
		{
			string url = ResolveUrl("UsageReport.aspx?id=" + businessPartnerId);
			ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openNewTab", "window.open('" + url + "','_blank');", true);
		}
	}
}