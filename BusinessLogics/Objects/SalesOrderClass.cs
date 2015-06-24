using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
  public class SalesOrderClass
  {
    public static salesOrder GetSalesOrder(String docNum, String companyCode, DALPortalDataContext dc)
    {
      return dc.salesOrders.Where(c => c.documentNumber.Equals(docNum) && c.companyCode.Equals(companyCode)).FirstOrDefault();
    }

		public static salesOrderLine GetSalesOrderLine(String docNum, Int32 lineNum, String companyCode, DALPortalDataContext dc)
		{
			salesOrder salesOrder = GetSalesOrder(docNum, companyCode, dc);

			if (salesOrder == null)
				return null;
			else
				return salesOrder.salesOrderLines.Where(c => c.lineNum.Equals(lineNum)).FirstOrDefault();
		}
  }
}
