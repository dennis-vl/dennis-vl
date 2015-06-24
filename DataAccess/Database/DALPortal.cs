using System;
using System.Linq;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.Database
{
  partial class DALPortalDataContext
	{
		#region item
		partial void Insertitem(item instance)
		{
			instance.timeStampCreated = DateTime.Now;

			String companyGroupCode = this.companyGroupForItems.Where(c => c.companyCode.Equals(instance.companyCode)).Select(c => c.companyGroupCode).FirstOrDefault();

			if (!String.IsNullOrEmpty(companyGroupCode))
			{
				instance.companyCode = null;
				instance.companyGroupCode = companyGroupCode;
			}

			this.ExecuteDynamicInsert(instance);
		}

		partial void Updateitem(item instance)
		{
			instance.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}
		#endregion

		#region businessPartner
		partial void InsertbusinessPartner(businessPartner instance)
		{
			instance.timeStampCreated = DateTime.Now;

			String companyGroupCode = this.companyGroupForBps.Where(c => c.companyCode.Equals(instance.companyCode)).Select(c => c.companyGroupCode).FirstOrDefault();

			if (!String.IsNullOrEmpty(companyGroupCode))
			{
				instance.companyCode = null;
				instance.companyGroupCode = companyGroupCode;
			}

			this.ExecuteDynamicInsert(instance);
		}

		partial void UpdatebusinessPartner(businessPartner instance)
		{
			instance.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}
		#endregion

		#region salesOrder
		partial void InsertsalesOrder(salesOrder instance)
		{
			instance.timeStampCreated = DateTime.Now;
			instance.docType = "SO";
			this.ExecuteDynamicInsert(instance);
		}

		partial void UpdatesalesOrder(salesOrder instance)
		{
			instance.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}
		#endregion

		#region salesOrderLine
		partial void InsertsalesOrderLine(salesOrderLine instance)
		{
			instance.docType = "SO";

			if (!String.IsNullOrEmpty(instance.uomCodeOrg))
				if (this.unitOfMeasureLinks.Any(c => c.uomCodeOrg.Equals(instance.uomCodeOrg)))
					instance.uomCode = this.unitOfMeasureLinks.Where(c => c.uomCodeOrg.Equals(instance.uomCodeOrg)).FirstOrDefault().uomCode;

			this.ExecuteDynamicInsert(instance);
		}

		partial void UpdatesalesOrderLine(salesOrderLine instance)
		{
			instance.salesOrder.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}

		#endregion

		#region delivery
		partial void Insertdelivery(delivery instance)
		{
			instance.timeStampCreated = DateTime.Now;
			instance.docType = "DL";

			this.ExecuteDynamicInsert(instance);
		}

		partial void Updatedelivery(delivery instance)
		{
			instance.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}
		#endregion

		#region deliveryLine
		partial void InsertdeliveryLine(deliveryLine instance)
		{
			instance.docType = "DL";

			if (!String.IsNullOrEmpty(instance.uomCodeOrg))
				if (this.unitOfMeasureLinks.Any(c => c.uomCodeOrg.Equals(instance.uomCodeOrg)))
					instance.uomCode = this.unitOfMeasureLinks.Where(c => c.uomCodeOrg.Equals(instance.uomCodeOrg)).FirstOrDefault().uomCode;

			if (instance.baseDocId != null && instance.baseLineNum != null)
			{ 
				// Copy LineInformation from base document
				if (instance.baseDocType.Equals("SO"))
				{
					salesOrderLine soLine = this.salesOrderLines.Where(c => c.docId.Equals(instance.baseDocId) && c.lineNum.Equals(instance.baseLineNum)).FirstOrDefault();
					if (soLine != null)
					{
						if (instance.itemId == null)
							instance.itemId = soLine.itemId;

						if (String.IsNullOrEmpty(instance.lineTypeCode))
							instance.lineTypeCode = soLine.lineTypeCode;

						if (instance.price == null)
							instance.price = soLine.price;

						if (String.IsNullOrEmpty(instance.customerReference))
							instance.customerReference = soLine.customerReference;

						if (String.IsNullOrEmpty(instance.uomCode))
							instance.uomCode = soLine.uomCode;

						if (String.IsNullOrEmpty(instance.uomCodeOrg))
							instance.uomCodeOrg = soLine.uomCodeOrg;

						if (String.IsNullOrEmpty(instance.uomCodeOrg))
							instance.uomCodeOrg = soLine.uomCodeOrg;

						if (String.IsNullOrEmpty(instance.customerItemCode))
							instance.customerItemCode = soLine.customerItemCode;
					}
					else
					{
						throw new Exception(String.Format("Base Sales order {0} line {1} does not exists!", instance.baseDocId, instance.baseLineNum));
					}
				}

			}

			this.ExecuteDynamicInsert(instance);
		}

		partial void UpdatedeliveryLine(deliveryLine instance)
		{
			instance.delivery.timeStampLastUpdate = DateTime.Now;
			this.ExecuteDynamicUpdate(instance);
		}

		#endregion
	}
}
