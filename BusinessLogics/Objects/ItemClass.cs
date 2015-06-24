using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
  public static class ItemClass
  {
    public static item GetItem(String itemCode, String companyCode, DALPortalDataContext dc)
    {
			if (!dc.items.Any(c => c.itemCode.Equals(itemCode)))
				return null;

      if (dc.companyGroupForItems.Any(c => c.companyCode.Equals(companyCode)))
        return dc.items.Where(c => c.itemCode.Equals(itemCode) && c.companyGroup.companyGroupForItems.Any(d => d.companyCode.Equals(companyCode))).FirstOrDefault();
      else
        return dc.items.Where(c => c.itemCode.Equals(itemCode) && c.companyCode.Equals(companyCode)).FirstOrDefault();
    }

		public static item CreateItem(String itemCode, String itemDescription, String companyCode)
		{
			DALPortalDataContext dc = new DALPortalDataContext();

			if (GetItem(itemCode, companyCode, dc) == null)
			{
				item newItem = new item();
				newItem.itemCode = itemCode;
				newItem.description = itemDescription;
				newItem.companyCode = companyCode;

				dc.items.InsertOnSubmit(newItem);
				dc.SubmitChanges();

				return newItem;
			}

			return null;
		}
  }
}
