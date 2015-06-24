using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.Database
{
	partial class item
	{
		partial void OnValidate(System.Data.Linq.ChangeAction action)
		{
			if ((this.companyCode == null && this.companyGroupCode == null) || (this.companyCode != null && this.companyGroupCode != null))
			{
				throw new Exception("companyCode or companyGroupCode must be filled!");
			}
		} 
	}
}
