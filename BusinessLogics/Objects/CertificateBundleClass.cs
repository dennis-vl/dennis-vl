using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
  public class CertificateBundleClass
  {
    public static certificateBundle GetCurrentBundle(Int32 businessPartnerId, DALPortalDataContext dc)
    {
      return dc.certificateBundles.Where(c => c.businessPartnerId.Equals(businessPartnerId)).Where(r => r.isActive).SingleOrDefault();
    }
  }
}
