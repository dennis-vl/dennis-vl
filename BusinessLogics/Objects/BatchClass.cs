using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects
{
	public class BatchClass
	{
		public static batch GetBatch(String batchNumber, String heatNumber, Int32 itemId, String companyCode, DALPortalDataContext dc)
		{
			return dc.batches.Where(c => c.batchNumber.Equals(batchNumber) && c.heatNumber.Equals(heatNumber) && c.itemId.Equals(itemId) && c.companyCode.Equals(companyCode)).FirstOrDefault();
		}

		public static void RemoveBatchesDocument(int docId, string docType, DALPortalDataContext dc)
		{
			var batchDocuments = dc.batchDocuments.Where(c => c.baseDocType.Equals(docType) && c.baseDocId.Equals(docId));
			List<int> batchIds = batchDocuments.Select(c => c.batchId).ToList();
			dc.batchDocuments.DeleteAllOnSubmit(batchDocuments);

			dc.SubmitChanges();

			var batches = dc.batches.Where(c => batchIds.Contains(c.batchId) && c.batchDocuments.Count() == 0);
			dc.batches.DeleteAllOnSubmit(batches);

			dc.SubmitChanges();
		}
	}
}
