using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VanLeeuwen.Framework.Xml;
using VanLeeuwen.Projects.WebPortal.DataAccess.CachedObjects;
using VanLeeuwen.Framework.Extensions;
using System.Diagnostics;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.SAPConnector
{
	public class SAPProcessor
	{
		private SapConnection con;
		private RfcRepository repo;
		private RfcDestination dest;

		public SAPProcessor()
		{ 
			// Initiate SAP Connection
			CreateConnection();
		}

		public Boolean CreateConnection()
		{
			try
			{
				this.con = new SapConnection();
				RfcDestinationManager.RegisterDestinationConfiguration(con);
				this.dest = RfcDestinationManager.GetDestination("NSP");
				this.repo = dest.Repository;
			}
			catch (Exception ex)
			{
				try
				{
					// Break connection
					RfcDestinationManager.UnregisterDestinationConfiguration(con);
				}
				catch (Exception ex2) { }
				throw ex;
			}

			return true;
		}

		public Boolean ProcessInvoice(String databaseName, String xmlContent)
		{
			Boolean returnValue = true;
			String errorMessage;
			String documentNumber;

			try
			{	
				DocumentCached invoice;
				if (!GetInvoiceFromXML(databaseName, xmlContent, out invoice, out errorMessage))
				{
					Trace.WriteLine(String.Format("Unable to get Invoice from XML. Error: {0}", errorMessage), "ProcessInvoice");
					returnValue = false;
				}

				if (!ImportInvoice(databaseName, invoice, out documentNumber, out errorMessage))
				{
					Trace.WriteLine(String.Format("Unable to create Invoice in SAP ECC6. Error: {0}", errorMessage), "ProcessInvoice");
					returnValue = false;
				}
			}
			finally
			{
				RfcDestinationManager.UnregisterDestinationConfiguration(con);
			}

			if(returnValue)
				Trace.WriteLine(String.Format("Created Invoice {0} in SAP ECC6 succesfully! ", documentNumber), "ProcessInvoice");

			return returnValue;
		}

		private Boolean GetInvoiceFromXML(String databaseName, String xmlContent, out DocumentCached invoice, out String errorMessage)
		{
			invoice = new DocumentCached();
			errorMessage = String.Empty;
			try
			{
				invoice = (DocumentCached)XmlToObject.CreateObject(xmlContent, invoice);
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return false;
			}

			return true;
		}

		private Boolean ImportInvoice(String databaseName, DocumentCached invoice, out String documentNumber, out String errorMessage)
		{
			errorMessage = String.Empty;
			documentNumber = String.Empty;

			try
			{
				IRfcFunction fReadTable = repo.CreateFunction("ZZBAPI_DEBIT_MEMO_REQUEST");
				fReadTable.SetValue("CUSTOMER", ("0000000" + invoice.CustomerCode).Right(10));
				fReadTable.SetValue("SALES_ORG", invoice.VLCompany);
				fReadTable.SetValue("PURCH_DATE", invoice.DocDate);
				fReadTable.SetValue("PURCH_NO_C", invoice.Comment);

				foreach (DocumentLineCached line in invoice.Lines)
				{
					RfcStructureMetadata metaData = dest.Repository.GetStructureMetadata("ZORDERLINE");
					IRfcStructure structConditions = metaData.CreateStructure();

					structConditions.SetValue("ITM_NUMBER", ("0000" + line.LineNum.ToString() + "0").Right(6));
					structConditions.SetValue("MATERIAL", line.ItemCode); // C => Certificate
					structConditions.SetValue("TARGET_QTY", line.Quantity);
					structConditions.SetValue("SALES_UNIT", line.UnitOfMeasure);
					structConditions.SetValue("COND_VALUE", line.Price);
					structConditions.SetValue("CURRENCY", line.Currency);
					structConditions.SetValue("SHORT_TEXT", line.ShortText);

					IRfcTable tblItems = fReadTable.GetTable("ORDERLINE");
					tblItems.Append(structConditions);
					fReadTable.SetValue("ORDERLINE", tblItems);
				}


				fReadTable.Invoke(dest);
				String result = (String)fReadTable.GetValue("SALESDOCUMENT");
				IRfcStructure result2 = (IRfcStructure)fReadTable.GetValue("RETURN");

				documentNumber = result.ToString();
				errorMessage = result2[3].ToString().Replace("FIELD MESSAGE=", "");


				if (String.IsNullOrEmpty(documentNumber) || !String.IsNullOrEmpty(errorMessage))
					return false;
				
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}

			return true;
		}		

		private Boolean ImportInvoice2(String databaseName, DocumentCached invoice)
		{

			IRfcFunction fReadTable = repo.CreateFunction("ZZBAPI_DEBIT_MEMO_REQUEST");
			fReadTable.SetValue("CUSTOMER", "0000008289");
			fReadTable.SetValue("SALES_ORG", "ZW01");
			fReadTable.SetValue("PURCH_DATE", DateTime.Now);
			fReadTable.SetValue("PURCH_NO_C", "TEST ZZBAPI_DEBIT_MEMO_REQUEST");

			RfcStructureMetadata metaData = dest.Repository.GetStructureMetadata("ZORDERLINE");
			IRfcStructure structConditions = metaData.CreateStructure();
			structConditions.SetValue("ITM_NUMBER", "000010");
			structConditions.SetValue("MATERIAL", "C"); // C => Certificate
			structConditions.SetValue("TARGET_QTY", 10);
			structConditions.SetValue("SALES_UNIT", "ST");
			structConditions.SetValue("COND_VALUE", 16);
			structConditions.SetValue("CURRENCY", "EUR");
			structConditions.SetValue("SHORT_TEXT", "test");

			IRfcTable tblItems = fReadTable.GetTable("ORDERLINE");
			tblItems.Append(structConditions);
			fReadTable.SetValue("ORDERLINE", tblItems);



			fReadTable.Invoke(dest);
			var result = fReadTable.GetValue("SALESDOCUMENT");
			var result2 = fReadTable.GetValue("RETURN");

			Console.WriteLine(result.ToString());
			Console.ReadLine();

			return true;
		}		
	}
}
