using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.CachedObjects
{
	public class DocumentCached
	{
		public String CustomerCode { get; set; }
		public String VLCompany { get; set; }
		public DateTime DocDate { get; set; }
		public String Comment { get; set; }

		public List<DocumentLineCached> Lines = new List<DocumentLineCached>();
	}
}
