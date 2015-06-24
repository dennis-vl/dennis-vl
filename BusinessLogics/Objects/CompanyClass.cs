using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
	public static class CompanyClass
	{
		public static String GetCompanyBasedOnSection(String sectionCode, DALPortalDataContext dc)
		{
			return dc.companySections.Where(c => c.CompanySectionCode.Equals(sectionCode)).Select(c => c.CompanyCode).FirstOrDefault();
		}

		public static String GetCompanyGroupCodeForItems(String companyCode, DALPortalDataContext dc)
		{
			return dc.companyGroupForItems.Where(c => c.companyCode.Equals(companyCode)).Select(c => c.companyGroupCode).FirstOrDefault();
		}

		public static String GetCompanyGroupCodeForBps(String companyCode, DALPortalDataContext dc)
		{
			return dc.companyGroupForBps.Where(c => c.companyCode.Equals(companyCode)).Select(c => c.companyGroupCode).FirstOrDefault();
		}
	}
}
