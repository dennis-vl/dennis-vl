using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.CachedObjects
{
	public class DocumentLineCached
	{
		public Int32 LineNum { get; set; }
		public String ItemCode { get; set; }
		public Decimal Quantity { get; set; }
		public String UnitOfMeasure { get; set; }
		public Decimal Price { get; set; }
		public String Currency { get; set; }
		public String ShortText { get; set; }
	}
}
