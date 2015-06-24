using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
  public class ContactPersonClass
  {
    public static contactPerson GetContactPerson(Guid userId, DALPortalDataContext dc)
    {
      return dc.contactPersons.Where(c => c.userId.Equals(userId)).FirstOrDefault();
    }
  }
}
