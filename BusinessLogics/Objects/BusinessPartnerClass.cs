using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
  public class BusinessPartnerClass
  {
    /// <summary>
    /// Get BusinessPartner based on BusinessPartnerCode and CompanyCode
    /// </summary>
    /// <param name="businessPartnerCode"></param>
    /// <param name="companyCode"></param>
    /// <param name="dc"></param>
    /// <returns></returns>
    public static businessPartner GetBusinessPartner(String businessPartnerCode, String companyCode, DALPortalDataContext dc)
    {
      if (!dc.businessPartners.Any(c => c.bpCode.Equals(businessPartnerCode)))
        return null;

      if (dc.companyGroupForBps.Any(c => c.companyCode.Equals(companyCode)))
        return dc.businessPartners.Where(c => c.bpCode.Equals(businessPartnerCode) && c.companyGroup.companyGroupForBps.Any(d => d.companyCode.Equals(companyCode))).FirstOrDefault();
      else
        return dc.businessPartners.Where(c => c.bpCode.Equals(businessPartnerCode) && c.companyCode.Equals(companyCode)).FirstOrDefault();
    }

    /// <summary>
    /// Get Mother BusinessPartner based on UserId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dc"></param>
    /// <returns></returns>
    public static businessPartner GetBusinessPartnerMother(Guid userId, DALPortalDataContext dc)
    {
      businessPartner bp = dc.businessPartners.Where(c => c.contactPersons.Any(d => d.userId.Equals(userId))).FirstOrDefault();
      if (bp == null)
        return null;

      if (bp.businessPartner1 == null)
        return bp;
      else
        return bp.businessPartner1;
    }
  }
}
