using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
  public class SalesOrderUpdater
  {
		//public static Boolean UpdateSalesOrder(PortalDocumentCached documentCached, SAPbobsCOM.Company company, DALDataContext dc)
		//{
		//	try
		//	{
		//		int result;
		//		var lines = dc.RDR1s.Where(c => c.DocEntry.Equals(documentCached.DocEntry)).Select(c => new { VisualLineNum = c.VisOrder, c.LineNum, c.U_LineNum, c.ItemCode });

		//		// Get current document
		//		IDocuments document = SalesOrderClass.GetSalesOrder(company, documentCached.DocEntry);

		//		// Linenum, Index
		//		Dictionary<Int32, Int32> lineNums = GetLineIndexes(document);

		//		document.NumAtCard = documentCached.CustRefNum;
		//		document.DocCurrency = documentCached.DocCurrency;
		//		document.DocRate = documentCached.CurrencyRate;
		//		document.DocDueDate = documentCached.RequiredDate;

		//		result = document.Update();
		//		if (result != 0)
		//			throw new Exception(company.GetLastErrorDescription());

		//		foreach (var line in documentCached.DocumentLinesCached)
		//		{
		//			// New Line
		//			if (line.LineNum == -1)
		//			{
		//				if (line.LineStatus == "C")
		//					continue;

		//				document.Lines.Add();
		//				document.Lines.ItemCode = line.ItemCode;
		//				document.Lines.Quantity = line.Quantity;
		//				document.Lines.UnitPrice = line.UnitPrice;
		//				document.Lines.Weight1 = line.Weight;
		//				//document.Lines.UoMEntry = -1;  // Voorlopig Handmatig
		//				document.Lines.MeasureUnit = line.UnitOfMeasure;
		//				document.Lines.ShipDate = line.RequiredDate;
		//				document.Lines.UserFields.Fields.Item("U_LineNum").Value = line.U_LineNum;
		//				document.Lines.UserFields.Fields.Item("U_SOType").Value = line.U_SOType;
		//			}
		//			else
		//			{
		//				document.Lines.SetCurrentLine(lineNums.Where(c => c.Key.Equals(line.LineNum)).SingleOrDefault().Value);

		//				//if (line.LineStatus == "O")
		//				//{
		//					// Update Line
		//					document.Lines.ItemCode = line.ItemCode;
		//					if (lines.Where(c => c.U_LineNum.Equals(line.U_LineNum)).FirstOrDefault().ItemCode != line.ItemCode)
		//					{
		//						result = document.Update();
		//						if (result != 0)
		//							throw new Exception(company.GetLastErrorDescription());
		//					}

		//					document.Lines.Quantity = line.Quantity;
		//					document.Lines.UnitPrice = line.UnitPrice;
		//					document.Lines.Weight1 = line.Weight;
		//					//document.Lines.UoMEntry = -1;  // Voorlopig Handmatig
		//					document.Lines.MeasureUnit = line.UnitOfMeasure;
		//					document.Lines.ShipDate = line.RequiredDate;
		//					document.Lines.UserFields.Fields.Item("U_LineNum").Value = line.U_LineNum;
		//					document.Lines.UserFields.Fields.Item("U_SOType").Value = line.U_SOType;

		//					//result = document.Update();
		//					//if (result != 0)
		//					//  throw new Exception(company.GetLastErrorDescription());
		//				//}
		//				//else
		//				//{
		//				//  // Close Line
		//				//  document.Lines.LineStatus = BoStatus.bost_Close;

		//				//  result = document.Update();
		//				//  if (result != 0)
		//				//    throw new Exception("Unable to close line " + document.Lines.LineNum.ToString() + ", " + company.GetLastErrorDescription());
		//				//}
		//			}
		//		}

		//		result = document.Update();
		//		if (result != 0)
		//			throw new Exception(company.GetLastErrorDescription());

		//		// Line is removed if line is not in idoc anymore
		//		var soLines = dc.RDR1s.Where(c => c.DocEntry.Equals(documentCached.DocEntry)).Select(c => new { U_LineNum = c.U_LineNum.Value, c.LineNum, c.LineStatus }).ToList();
		//		List<Int32> linesInIdoc = documentCached.DocumentLinesCached.Select(c => c.U_LineNum).ToList();

		//		foreach (var line in soLines.Where(c => !linesInIdoc.Contains(c.U_LineNum)))
		//		{
		//			// Linenum, Index
		//			Dictionary<Int32, Int32> lineIndexes = GetLineIndexes(document);

		//			if (line.LineStatus.Value != 'C')
		//			{
		//				// Remove line
		//				document.Lines.SetCurrentLine(lineIndexes.Where(c => c.Key.Equals(line.LineNum)).SingleOrDefault().Value);
		//				document.Lines.Delete();

		//				result = document.Update();
		//				if (result != 0)
		//					throw new Exception(company.GetLastErrorDescription());
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Trace.WriteLine(String.Format("Update Sales Order {0} Failed. Error: {1}. {2}", documentCached.U_DocNum, ex.Message, ex.InnerException), "UpdateSalesOrder");
		//		return false;
		//	}

		//	Trace.WriteLine(String.Format("Update Sales Order {0} Succes in company {1}.", documentCached.U_DocNum, company.CompanyDB), "UpdateSalesOrder");
		//	return true;
		//}

		//private static Dictionary<int, int> GetLineIndexes(IDocuments document)
		//{
		//	// Get Linenums (LineNum, Index)
		//	Dictionary<Int32, Int32> lineNums = new Dictionary<Int32, Int32>();
		//	for (int i = 0; i < document.Lines.Count; i++)
		//	{
		//		document.Lines.SetCurrentLine(i);
		//		lineNums.Add(document.Lines.LineNum, i);
		//	}
		//	return lineNums;
		//}
  }
}
