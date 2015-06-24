using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.Objects;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
	public class XMLSAPECC6Processor
	{
		public static Boolean ProcessCustomer(string fileContent, out businessPartner businessPartner, out String outCompanyCode)
		{
			businessPartner = new businessPartner();

			DALPortalDataContext dc = new DALPortalDataContext();
			Boolean update;
			String companyCode = String.Empty;
			outCompanyCode = String.Empty;

			try
			{
				XDocument xmldoc = XDocument.Parse(fileContent);
				XElement docTypeElement = xmldoc.Element("DEBMAS03");
				XElement iDocElement = docTypeElement.Element("IDOC");
				var customerHeaderElement = iDocElement.Element("E1KNA1M");
				//Int32 CardCode;

				// Remove unused elements
				iDocElement.Element("EDI_DC40").Remove();

				// BpCode, remove leading zero's from Cardcode
				String bpCode = Convert.ToInt32(customerHeaderElement.Element("KUNNR").Value).ToString();
				String sectionCode = customerHeaderElement.Element("E1KNVVM").Element("VKORG").Value;

				companyCode = CompanyClass.GetCompanyBasedOnSection(sectionCode, dc);
				outCompanyCode = companyCode;

				// Check if bp Already exists
				businessPartner = BusinessPartnerClass.GetBusinessPartner(bpCode, companyCode, dc);
				if (businessPartner == null)
				{
					businessPartner = new businessPartner();
					update = false;
				}
				else
				{
					update = true;
				}

				if (!update)
				{
					String companyGroupCode = dc.companyGroupForBps.Where(c => c.companyCode.Equals(companyCode)).Select(c => c.companyGroupCode).FirstOrDefault();

					if (String.IsNullOrEmpty(companyGroupCode))
						businessPartner.companyCode = companyCode;
					else
						businessPartner.companyGroupCode = companyGroupCode;
				}

				// Mapp customer header data
				businessPartner.bpCode = bpCode;
				businessPartner.bpType = 'C';
				businessPartner.bpName = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("NAME1")).FirstOrDefault().Value;
				businessPartner.address = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("STRAS")).FirstOrDefault().Value;

				if (customerHeaderElement.Elements().Any(c => c.Name.ToString().Equals("TELF1")))
					businessPartner.telephone = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("TELF1")).FirstOrDefault().Value;

				if(customerHeaderElement.Elements().Any(c => c.Name.ToString().Equals("TELFX")))
					businessPartner.fax = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("TELFX")).FirstOrDefault().Value;
				
				businessPartner.countryCode = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("LAND1")).FirstOrDefault().Value;
				businessPartner.languageCode = customerHeaderElement.Elements().Where(c => c.Name.ToString().Equals("SPRAS_ISO")).FirstOrDefault().Value;


				// Mapp contactperson data
				foreach (var contactpersonElement in customerHeaderElement.Elements("E1KNVKM").ToList())
				{
					contactPerson contact;

					// Contact Id
					String contactPersonCode = Convert.ToInt32(contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("PARNR")).FirstOrDefault().Value).ToString();
					Boolean newContact = false;

					if (update && businessPartner.contactPersons.Any(c => c.contactPersonCode.Equals(contactPersonCode)))
					{
						contact = businessPartner.contactPersons.Where(c => c.contactPersonCode.Equals(contactPersonCode)).FirstOrDefault();
					}
					else
					{
						contact = new contactPerson();
						contact.contactPersonCode = contactPersonCode;
						newContact = true;
					}

					// Email
					var emailAddress = contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("PARAU")).FirstOrDefault();
					if (emailAddress != null)
						contact.eMail = emailAddress.Value.ToLower();

					// Title
					var title = contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("ANRED")).FirstOrDefault();
					if (title != null)
						contact.title = title.Value;

					// FirstName
					var firstName = contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("NAMEV")).FirstOrDefault();
					if (firstName != null)
						contact.firstName = firstName.Value;

					// LastName
					var lastName = contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("NAME1")).FirstOrDefault();
					if (lastName != null)
						contact.lastName = lastName.Value;

					// WebUser
					var webUserType = contactpersonElement.Elements().Where(c => c.Name.ToString().Equals("AKVER")).FirstOrDefault();
					if (webUserType != null)
					{
						contact.isWebContact = (webUserType.Value == "02") ? false : true;
						contact.TMP_PortalAccess = webUserType.Value;
					}

					// Add CardCode to contactperson
					if (newContact)
						businessPartner.contactPersons.Add(contact);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("Error converting customer {0} for company {1} from SAP ECC6 XML format. Error: {2}", businessPartner.bpCode, companyCode, ex.Message), "MapCustomerFromECC6");
				return false;
			}

			if (update)
			{
				try
				{
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Updating Customer {0} for company {1} Successfull.", businessPartner.bpCode, companyCode), "MapCustomerFromECC6");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Updating Customer {0} for company {1}. Error: {2}", businessPartner.bpCode, companyCode, ex.Message), "MapCustomerFromECC6");
					return false;
				}
			}
			else
			{
				try
				{
					dc.businessPartners.InsertOnSubmit(businessPartner);
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Creating Customer {0} for company {1} Successfull.", businessPartner.bpCode, companyCode), "MapCustomerFromECC6");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Creating Customer {0} for company {1}. Error: {2}", businessPartner.bpCode, companyCode, ex.Message), "MapCustomerFromECC6");
					return false;
				}
			}

			return true;
		}


		public static Boolean ProcessDelivery(String fileContent, out delivery deliveryDocument)
		{
			deliveryDocument = new delivery();
			List<batch> batches = new List<batch>();
			String sapECC6DocNum = String.Empty;

			DALPortalDataContext dc = new DALPortalDataContext();
			Boolean update;
			Boolean returnValue = true;

			try
			{
				List<String> items = new List<String>();

				XDocument xmldoc = XDocument.Parse(fileContent);
				XElement docTypeElement = xmldoc.Element("DELVRY03");
				XElement iDocElement = docTypeElement.Element("IDOC");
				XElement deliveryElement = iDocElement.Element("E1EDL20");

				// Company
				String sectionCode = deliveryElement.Element("VKORG").Value;
				String companyCode = CompanyClass.GetCompanyBasedOnSection(sectionCode, dc);


				sapECC6DocNum = RemoveLeadingZeros(deliveryElement.Element("VBELN").Value);

				// Check if Sales Order already exists
				deliveryDocument = dc.deliveries.Where(c => c.documentNumber.Equals(sapECC6DocNum) && c.companyCode.Equals(companyCode)).FirstOrDefault();
				if (deliveryDocument == null)
				{
					deliveryDocument = new delivery();
					deliveryDocument.companyCode = companyCode;
					deliveryDocument.documentNumber = sapECC6DocNum;
					deliveryDocument.docStatusCode = "O";
					update = false;
				}
				else
				{
					update = true;
				}

				// Document Header info
				String sapECC6BusinessPartnerCode = RemoveLeadingZeros(deliveryElement.Elements("E1ADRM1").Where(c => c.Element("PARTNER_Q").Value.Equals("AG")).FirstOrDefault().Element("PARTNER_ID").Value);
				businessPartner bp = Objects.BusinessPartnerClass.GetBusinessPartner(sapECC6BusinessPartnerCode, deliveryDocument.companyCode, dc);
				if (bp == null)
					throw new Exception(String.Format("BusinessPartner {0} does not exists.", sapECC6BusinessPartnerCode));


				// Get BusinessPartnerID
				deliveryDocument.businessPartnerId = bp.businessPartnerId;

				// Dates
				XElement delDateElement = deliveryElement.Elements("E1EDT13").ToList().Where(c => c.Element("QUALF").Value.Equals("007")).FirstOrDefault();
				deliveryDocument.deliveryDate = DateTime.ParseExact(delDateElement.Element("NTEND").Value, "yyyyMMdd", CultureInfo.CurrentCulture);
				

				XElement del2DateElement = deliveryElement.Elements("E1EDT13").ToList().Where(c => c.Element("QUALF").Value.Equals("015")).FirstOrDefault();
				deliveryDocument.docDate = DateTime.ParseExact(del2DateElement.Element("NTEND").Value, "yyyyMMdd", CultureInfo.CurrentCulture);
				deliveryDocument.createDate = DateTime.ParseExact(del2DateElement.Element("NTEND").Value, "yyyyMMdd", CultureInfo.CurrentCulture);

				if (deliveryDocument.deliveryDate < DateTime.Now.AddYears(-50))
					deliveryDocument.deliveryDate = deliveryDocument.docDate;

				//// Delivery lines
				List<XElement> deliveryLines = deliveryElement.Elements("E1EDL24").Where(c => c.Element("HIPOS") == null).ToList();
				List<XElement> deliverySubLines = deliveryElement.Elements("E1EDL24").Where(c => c.Element("HIPOS") != null).ToList();

				foreach (var deliveryLineElement in deliveryLines)
				{
					deliveryLine line = new deliveryLine();

					String lineNumString = deliveryLineElement.Element("POSNR").Value;
					Int32 lineNum = Convert.ToInt32(lineNumString);

					Boolean newLine = false;
					if (update && deliveryDocument.deliveryLines.Any(c => c.lineNum.Equals(lineNum)))
					{
						line = deliveryDocument.deliveryLines.Where(c => c.lineNum.Equals(lineNum)).FirstOrDefault();
					}
					else
					{
						line.lineNum = lineNum;
						newLine = true;
					}

					// DeliveryLine is not connected to a Sales Order
					if (deliveryLineElement.Element("VGBEL") == null)
						continue;

					String sapSODocNum = RemoveLeadingZeros(deliveryLineElement.Element("VGBEL").Value);
					Int32 sapSOLineNum = Convert.ToInt32(deliveryLineElement.Element("VGPOS").Value);

					salesOrderLine salesOrderLine = Objects.SalesOrderClass.GetSalesOrderLine(sapSODocNum, lineNum, companyCode, dc);

					if (salesOrderLine == null)
					{
						Trace.WriteLine(String.Format("Cannot find Sales Order {0}, Line {1} for Delivery {2}.", sapSODocNum, sapSOLineNum, sapECC6DocNum), "MapDeliveryXMLToSBO");
						returnValue = false;
						continue;
					}

					String itemCode = RemoveLeadingZeros(deliveryLineElement.Element("MATNR").Value);
					item item = Objects.ItemClass.GetItem(itemCode, companyCode, dc);

					// Get POSTYPE
					String posType = deliveryLineElement.Element("E1EDL26").Element("PSTYV").Value;
					XElement baseLineNum = deliveryLineElement.Element("HIPOS");

					// Add Batchinfo
					Decimal quantity = Convert.ToDecimal(deliveryLineElement.Element("LFIMG").Value, NumberFormatInfo.InvariantInfo);

					if (lineNumString.Substring(0, 1).Equals("9"))
						continue;

					if (Serac.BasicFunctions.Right(itemCode, 1).Equals("B") && (posType == "ZBNI" || posType == "ZCNI"))
					{
						// Get Quantity and Batches from Treatment line
						GetBatchesTreatmentLine(ref batches, ref quantity, line.lineNum, deliverySubLines, item.itemId);
					}
					else if (quantity > 0)
					{
						GetBatch(ref batches, line.lineNum, deliveryLineElement, quantity, item.itemId);
					}
					else
					{
						// Get Quantity and Batches from "Charge Splitsing"
						GetBatchesChargeSplitsing(ref batches, ref quantity, line.lineNum, deliverySubLines, item.itemId);
					}

					// Update Currency from base document
					if (deliveryDocument.currencyCode == null)
					{
						deliveryDocument.currencyCode = salesOrderLine.salesOrder.currencyCode;
						deliveryDocument.currencyRate = salesOrderLine.salesOrder.currencyRate;
					}

					line.baseDocId = salesOrderLine.salesOrder.docId;
					line.baseLineNum = Convert.ToInt32(deliveryLineElement.Element("VGPOS").Value);
					line.baseDocType = "SO";

					line.quantity = quantity;
					line.itemId = item.itemId;
					line.lineStatusCode = "O";
					line.itemDescription = salesOrderLine.itemDescription;

					// Get Certificates as ordered from base document
					line.certOrdered = salesOrderLine.certOrdered;

					// Add Line to SBO delivery line
					if (newLine)
						deliveryDocument.deliveryLines.Add(line);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("Error Converting Delivery XML to WebPortal format. Error: {0}", ex.Message), "ProcessDelivery");
				return false;
			}

			if (deliveryDocument.deliveryLines.Count == 0)
				return false;

			if (update)
			{
				try
				{
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Updating Delivery {0} for company {1} Successfull.", deliveryDocument.documentNumber, deliveryDocument.companyCode), "ProcessDelivery");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Updating Delivery {0} for company {1}. Error: {2}", deliveryDocument.documentNumber, deliveryDocument.companyCode, ex.Message), "ProcessDelivery");
					return false;
				}
			}
			else
			{
				try
				{
					dc.deliveries.InsertOnSubmit(deliveryDocument);
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Creating Delivery {0} for company {1} Successfull.", deliveryDocument.documentNumber, deliveryDocument.companyCode), "ProcessDelivery");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Creating Delivery {0} for company {1}. Error: {2}", deliveryDocument.documentNumber, deliveryDocument.companyCode, ex.Message), "ProcessDelivery");
					return false;
				}
			}

			if (!ProcessBatches(deliveryDocument, batches))
				return false;

			return returnValue;
		}

		private static Boolean ProcessBatches(delivery deliveryDocument, List<batch> batches)
		{
			DALPortalDataContext dc = new DALPortalDataContext();
			Boolean returnValue = true;

			// Remove all batches attached to this document
			BatchClass.RemoveBatchesDocument(deliveryDocument.docId, deliveryDocument.docType, dc);

			foreach (batch batchLine in batches)
			{
				try
				{
					batch batch = BatchClass.GetBatch(batchLine.batchNumber, batchLine.heatNumber, batchLine.itemId, deliveryDocument.companyCode, dc);

					Boolean newBatch = false;
					if (batch == null)
					{
						batch = new batch();
						newBatch = true;
					}

					if (newBatch)
					{
						batch.batchNumber = batchLine.batchNumber;
						batch.companyCode = deliveryDocument.companyCode;
					}

					batch.itemId = batchLine.itemId;
					batch.certificateIndexNumber = batchLine.certificateIndexNumber;
					batch.heatNumber = batchLine.heatNumber;
					batch.ixosArchiveId = batchLine.ixosArchiveId;

					if (newBatch)
					{
						// Check if certificate is already on the server
						String fileName = "CERT_" + batchLine.ixosArchiveId + ".pdf";
						String filePath = Path.Combine(Parameters_DataProcessor.CompanyFilesRoot, "Certificates", fileName);
						String dbPath = Path.Combine(@"~\Files\Certificates", fileName);

						if (File.Exists(filePath))
							batch.certificateLink = dbPath;
					}

					foreach (batchDocument batchDocLine in batchLine.batchDocuments)
					{
						// Check if Batch relates to document
						batchDocument batchDoc = batch.batchDocuments.Where(c => c.baseDocId.Equals(deliveryDocument.docId) && c.baseLineNum.Equals(batchDocLine.baseLineNum) && c.baseDocType.Equals("DL")).FirstOrDefault();
						Boolean newBatchDoc = false;
						if (batchDoc == null)
						{
							newBatchDoc = true;
							batchDoc = new batchDocument();
							batchDoc.baseDocId = deliveryDocument.docId;
							batchDoc.baseLineNum = batchDocLine.baseLineNum;
							batchDoc.baseDocType = "DL";
						}

						batchDoc.quantity = batchDocLine.quantity;

						if (newBatchDoc)
							batch.batchDocuments.Add(batchDoc);
					}

					if (newBatch)
						dc.batches.InsertOnSubmit(batch);

					dc.SubmitChanges();
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Processing Batches for Delivery {0} for company {1}. Error: {2}", deliveryDocument.documentNumber, deliveryDocument.companyCode, ex.Message), "ProcessBatches");
					returnValue = false;
				}
			}

			return returnValue;
		}

		private static void GetBatchesChargeSplitsing(ref List<batch> batches, ref Decimal quantity, Int32 lineNum, List<XElement> deliverySubLines, Int32 itemId)
		{
			quantity = 0;
			List<XElement> chargeLines = deliverySubLines.Where(c => c.Element("POSNR").Value.Substring(0, 1).Equals("9")).ToList();
			foreach (XElement line in chargeLines)
			{
				if (Convert.ToInt32(line.Element("HIPOS").Value) != lineNum)
					continue;

				Decimal batchQuantity = Convert.ToDecimal(line.Element("LFIMG").Value, NumberFormatInfo.InvariantInfo);
				GetBatch(ref batches, lineNum, line, batchQuantity, itemId);

				quantity += batchQuantity;
			}
		}

		private static void GetBatchesTreatmentLine(ref List<batch> batches, ref Decimal quantity, Int32 lineNum, List<XElement> deliverySubLines, Int32 itemId)
		{
			List<XElement> treatmentLines = deliverySubLines.Where(c => c.Element("POSNR").Value.ToString().Substring(0, 1).StartsWith("0")).ToList();
			foreach (XElement line in deliverySubLines)
			{
				String currentLineNum = line.Element("POSNR").Value;

				if (Convert.ToInt32(line.Element("HIPOS").Value) != lineNum)
					continue;

				Decimal batchQuantity = Convert.ToDecimal(line.Element("LFIMG").Value, NumberFormatInfo.InvariantInfo);
				if (batchQuantity > 0)
					GetBatch(ref batches, lineNum, line, quantity, itemId);
				else
					GetBatchesChargeSplitsing(ref batches, ref quantity, lineNum, deliverySubLines, itemId);
			}
		}

		private static void GetBatch(ref List<batch> batches, Int32 lineNum, XElement deliveryLineElement, Decimal quantity, Int32 itemId)
		{
			String batchNumber = String.Empty;
			String heatNumber = String.Empty;
			String certificateIndexNr = String.Empty;

			var batchNumberElement = deliveryLineElement.Elements("E1EDL43").Where(c => c.Element("QUALF").Value.Equals("X")).FirstOrDefault();
			if (batchNumberElement != null)
				batchNumber = batchNumberElement.Element("BELNR").Value;

			if (!String.IsNullOrEmpty(batchNumber))
			{
				var heatNumberElement = deliveryLineElement.Elements("E1EDL43").Where(c => c.Element("QUALF").Value.Equals("Y")).FirstOrDefault();
				if (heatNumberElement != null)
				{
					var heatNumberElement2 = heatNumberElement.Element("BELNR");
					if(heatNumberElement2 != null)
						heatNumber = heatNumberElement2.Value;
				}

				var certificateIndexElement = deliveryLineElement.Elements("E1EDL43").Where(c => c.Element("QUALF").Value.Equals("Z")).FirstOrDefault();
				if (certificateIndexElement != null)
				{
					var certificateIndexElement2 = certificateIndexElement.Element("BELNR");
					if(certificateIndexElement2 != null)
						certificateIndexNr = certificateIndexElement2.Value;
				}

				String certificateDocID = String.Empty;
				if (deliveryLineElement.Element("MFRPN") != null)
					certificateDocID = deliveryLineElement.Element("MFRPN").Value;

				batch batch = new batch();
				batch.itemId = itemId;
				batch.batchNumber = batchNumber;
				batch.heatNumber = heatNumber;
				batch.ixosArchiveId = certificateDocID;
				batch.certificateIndexNumber = certificateIndexNr;

				batch.batchDocuments.Add(new batchDocument() { baseLineNum = lineNum, quantity = quantity });
				batches.Add(batch);
			}
		}

		private static String GetSOLineDeliveryDate(XElement orderLineElement)
		{
			List<String> deliveryDates = orderLineElement.Elements("E1EDP20").Select(c => c.Element("EDATU").Value).ToList();
			if (deliveryDates.Count() == 0)
				return String.Empty;

			String currentDate = DateTime.Now.ToString("yyyyMMdd");

			if (Convert.ToInt32(currentDate) > Convert.ToInt32(deliveryDates.Max()))
				return deliveryDates.Max();
			else
				return deliveryDates.Where(c => Convert.ToInt32(c) > Convert.ToInt32(currentDate)).Min();
		}

		public static Boolean ProcessSalesOrder(string fileContent, out salesOrder document)
		{
			document = new salesOrder();

			DALPortalDataContext dc = new DALPortalDataContext();
			Boolean update;

			try
			{
				XDocument xmldoc = XDocument.Parse(fileContent);
				XElement docTypeElement = xmldoc.Element("ORDERS05");
				XElement iDocElement = docTypeElement.Element("IDOC");

				// Company
				XElement companyElement = iDocElement.Elements("E1EDK14").ToList().Where(c => c.Element("QUALF").Value.Equals("008")).FirstOrDefault();
				String sectionCode = companyElement.Element("ORGID").Value;
				String companyCode = CompanyClass.GetCompanyBasedOnSection(sectionCode, dc);

				String ecc6DocNum = RemoveLeadingZeros(iDocElement.Element("E1EDK01").Element("BELNR").Value);

				// Check if Sales Order already exists
				document = dc.salesOrders.Where(c => c.documentNumber.Equals(ecc6DocNum) && c.companyCode.Equals(companyCode)).FirstOrDefault();
				if (document == null)
				{
					document = new salesOrder();
					document.companyCode = companyCode;
					document.documentNumber = ecc6DocNum;
					document.docStatusCode = "O";
					update = false;
				}
				else
				{
					update = true;
				}


				// Document Header info
				String sapECC6BusinessPartnerCode = Convert.ToInt32(iDocElement.Element("E1EDK01").Element("RECIPNT_NO").Value).ToString();
				businessPartner bp = Objects.BusinessPartnerClass.GetBusinessPartner(sapECC6BusinessPartnerCode, document.companyCode, dc);
				if (bp == null)
					throw new Exception(String.Format("BusinessPartner {0} does not exists.", sapECC6BusinessPartnerCode));

				document.businessPartnerId = bp.businessPartnerId;
				document.currencyCode = iDocElement.Element("E1EDK01").Element("CURCY").Value;
				document.currencyRate = Convert.ToDecimal(iDocElement.Element("E1EDK01").Element("WKURS").Value, NumberFormatInfo.InvariantInfo);

				// Customer Reference
				XElement custRef = iDocElement.Elements("E1EDK02").ToList().Where(c => c.Element("QUALF").Value.Equals("001")).FirstOrDefault();
				document.customerReference = custRef.Element("BELNR").Value;

				// Dates
				XElement delDateElement = iDocElement.Elements("E1EDK03").ToList().Where(c => c.Element("IDDAT").Value.Equals("002")).FirstOrDefault();
				document.deliveryDate = DateTime.ParseExact(delDateElement.Element("DATUM").Value, "yyyyMMdd", CultureInfo.CurrentCulture);

				XElement del2DateElement = iDocElement.Elements("E1EDK03").ToList().Where(c => c.Element("IDDAT").Value.Equals("012")).FirstOrDefault();
				document.docDate = DateTime.ParseExact(del2DateElement.Element("DATUM").Value, "yyyyMMdd", CultureInfo.CurrentCulture);

				XElement del3DateElement = iDocElement.Elements("E1EDK03").ToList().Where(c => c.Element("IDDAT").Value.Equals("025")).FirstOrDefault();
				document.createDate = DateTime.ParseExact(del3DateElement.Element("DATUM").Value, "yyyyMMdd", CultureInfo.CurrentCulture);

				// Sales Order lines
				foreach (var orderLineElement in iDocElement.Elements("E1EDP01").ToList())
				{
					salesOrderLine line = new salesOrderLine();

					Int32 lineNum = Convert.ToInt32(orderLineElement.Element("POSEX").Value);

					Boolean newLine = false;
					if (update && document.salesOrderLines.Any(c => c.lineNum.Equals(lineNum)))
					{
						line = document.salesOrderLines.Where(c => c.lineNum.Equals(lineNum)).FirstOrDefault();
					}
					else
					{
						line.lineNum = lineNum;
						newLine = true;
					}

					// Get Item (remove leading zero's)
					XElement ItemCodeElement = orderLineElement.Elements("E1EDP19").ToList().Where(c => c.Element("QUALF").Value.Equals("002")).FirstOrDefault();

					String itemCode = RemoveLeadingZeros(ItemCodeElement.Element("IDTNR").Value);
					String itemName = ItemCodeElement.Element("KTEXT").Value;

					item item = ItemClass.GetItem(itemCode, companyCode, dc);
					if (item == null)
						item = ItemClass.CreateItem(itemCode, itemName, companyCode);

					line.itemId = item.itemId;
					line.lineNum = Convert.ToInt32(orderLineElement.Element("POSEX").Value);
					line.uomCodeOrg = orderLineElement.Element("MENEE").Value;
					line.price = Convert.ToDecimal(orderLineElement.Element("VPREI").Value, NumberFormatInfo.InvariantInfo);
					line.itemDescription = itemName;

					if (orderLineElement.Element("MENGE") != null)
						line.quantity = Convert.ToDecimal(orderLineElement.Element("MENGE").Value, NumberFormatInfo.InvariantInfo);
					else
						line.quantity = 0;

					if (orderLineElement.Element("NTGEW") != null)
						line.weight = Convert.ToDecimal(orderLineElement.Element("NTGEW").Value, NumberFormatInfo.InvariantInfo);
					else
						line.weight = 0;

					line.lineTypeCode = orderLineElement.Element("PSTYV").Value;
					line.lineStatusCode = "O";

					line.customerReference = String.Empty;
					line.customerItemCode = String.Empty;


					XElement customerItemCodeElement = orderLineElement.Elements("E1EDP19").ToList().Where(c => c.Element("QUALF").Value.Equals("001")).FirstOrDefault();
					if (customerItemCodeElement != null)
					{
						XElement customerItemCodeElement2 = customerItemCodeElement.Element("IDTNR");
						if (customerItemCodeElement2 != null)
							line.customerItemCode = RemoveLeadingZeros(customerItemCodeElement2.Value);
					}

					XElement customerReference = orderLineElement.Elements("E1EDPT1").ToList().Where(c => c.Element("TDID").Value.Equals("0002")).FirstOrDefault();
					if (customerReference != null)
					{
						XElement customerReference2 = customerReference.Element("E1EDPT2");
						if (customerReference2 != null)
							line.customerReference = customerReference2.Element("TDLINE") != null ? customerReference2.Element("TDLINE").Value : String.Empty;
					}

					// Line is cancelled when field ABGRU is filled
					var lineStatusElement = orderLineElement.Element("ABGRU"); ;
					if (lineStatusElement != null)
						if (!String.IsNullOrWhiteSpace(lineStatusElement.Value))
							line.lineStatusCode = "C";

					// ShipDate
					String deliveryDate = GetSOLineDeliveryDate(orderLineElement);
					if (!String.IsNullOrEmpty(deliveryDate))
						line.deliveryDate = DateTime.ParseExact(deliveryDate, "yyyyMMdd", CultureInfo.CurrentCulture);

					// Certificate as Ordered
					Boolean certAsOrdered = orderLineElement.Elements("E1EDP02").ToList().Any(c => c.Element("QUALF").Value.Equals("902") && c.Element("BELNR").Value.Equals("ZCPT"));
					line.certOrdered = certAsOrdered;
					

					if (newLine)
						document.salesOrderLines.Add(line);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("Error converting Sales Order {0} for company {1} from SAP ECC6 XML format. Error: {2}", document.documentNumber, document.companyCode, ex.Message), "ProcessSalesOrder");
				return false;
			}

			if (update)
			{
				try
				{
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Updating Sales Order {0} for company {1} Successfull.", document.documentNumber, document.companyCode), "ProcessSalesOrder");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Updating Sales Order {0} for company {1}. Error: {2}", document.documentNumber, document.companyCode, ex.Message), "ProcessSalesOrder");
					return false;
				}
			}
			else
			{
				try
				{
					dc.salesOrders.InsertOnSubmit(document);
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Creating Sales Order {0} for company {1} Successfull.", document.documentNumber, document.companyCode), "ProcessSalesOrder");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Creating Sales Order {0} for company {1}. Error: {2}", document.documentNumber, document.companyCode, ex.Message), "ProcessSalesOrder");
					return false;
				}
			}

			return true;
		}

		public static bool ProcessItem(string fileContent)
		{
			item item = new item();

			DALPortalDataContext dc = new DALPortalDataContext();
			Boolean update;
			String companyCode = String.Empty;
			//outCompanyCode = String.Empty;

			try
			{
				XDocument xmldoc = XDocument.Parse(fileContent);
				XElement docTypeElement = xmldoc.Element("MATMAS05");
				XElement iDocElement = docTypeElement.Element("IDOC");
				var itemHeaderElement = iDocElement.Element("E1MARAM");

				// ItemCode, remove leading zero's from ItemCode
				String itemCode = RemoveLeadingZeros(itemHeaderElement.Element("MATNR").Value).ToString();
				String sectionCode = itemHeaderElement.Element("E1MVKEM").Element("VKORG").Value;

				companyCode = CompanyClass.GetCompanyBasedOnSection(sectionCode, dc);

				// Check if item Already exists
				item = ItemClass.GetItem(itemCode, companyCode, dc);
				if (item == null)
				{
					item = new item();
					update = false;
				}
				else
				{
					update = true;
				}

				if (!update)
					item.companyCode = companyCode;

				// Mapp customer header data
				item.itemCode = itemCode;

				// English description
				XElement ItemDescElement = itemHeaderElement.Elements("E1MAKTM").ToList().Where(c => c.Element("MSGFN").Value.Equals("005") && c.Element("SPRAS_ISO").Value.Equals("EN")).FirstOrDefault();
				if (ItemDescElement != null)
				{
					XElement ItemDescElement2 = ItemDescElement.Element("MAKTX");
					if (ItemDescElement2 != null)
						item.description = ItemDescElement2.Value;
				}
				
				// Certificate Type. 
				// 00 = Cert. Niet van toepassing
				if (itemHeaderElement.Elements().Any(c => c.Name.ToString().Equals("GTIN_VARIANT")))
					item.CertificateType = itemHeaderElement.Element("GTIN_VARIANT").Value;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("Error converting item {0} for company {1} from SAP ECC6 XML format. Error: {2}", item.itemCode, companyCode, ex.Message), "ProcessItem");
				return false;
			}

			if (update)
			{
				try
				{
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Updating Item {0} for company {1} Successfull.", item.itemCode, companyCode), "ProcessItem");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Updating Item {0} for company {1}. Error: {2}", item.itemCode, companyCode, ex.Message), "ProcessItem");
					return false;
				}
			}
			else
			{
				try
				{
					dc.items.InsertOnSubmit(item);
					dc.SubmitChanges();
					Trace.WriteLine(String.Format("Creating Item {0} for company {1} Successfull.", item.itemCode, companyCode), "ProcessItem");
				}
				catch (Exception ex)
				{
					Trace.WriteLine(String.Format("Error Creating Item {0} for company {1}. Error: {2}", item.itemCode, companyCode, ex.Message), "ProcessItem");
					return false;
				}
			}

			return true;
		}

		private static String RemoveLeadingZeros(String text)
		{
			Int32 textInt;
			Int32.TryParse(text, out textInt);
			if (textInt != 0)
				text = textInt.ToString();

			return text;
		}
	}
}
