IF EXISTS (SELECT name FROM sysobjects
					 WHERE name = 'vw_DeliveryLines' AND type = 'V') 
	DROP VIEW [dbo].vw_DeliveryLines
GO

CREATE VIEW [dbo].vw_DeliveryLines
AS
	-- ====================================================================================
	-- Name:    	  vw_DeliveryLines
	-- Author:      William van Aert
	-- 
	-- Version:     20140326.01
	--
	-- Description: View for certificates including Batches
	-- ====================================================================================

	SELECT 
		T7.bpCode AS CardCode, T2.documentNumber AS SODocNum, T3.lineNum AS SOLineNum,
		T0.documentNumber AS DELDocNum, T1.lineNum AS DELLineNum,
		T0.docDate AS DELDocDate, T2.customerReference AS CustomerReference,
		LTRIM(ISNULL(T3.customerItemCode, '') + '  ' + ISNULL(T3.customerReference, '')) AS SOLineReference, 
		T6.itemCode AS ItemCode,
		T6.description AS ItemDescription,
		CASE WHEN T5.heatNumber = '#' THEN 'No heat number required' ELSE T5.heatNumber END AS HeatNumber,
		CASE WHEN isnull(T5.CertificateLink, '') = '' 
			THEN '' 
			ELSE '~\Files\Certificates\' + 'CERT_' + T5.ixosArchiveId + '.pdf' 
		END AS CertificateLink
	FROM dbo.delivery T0
		INNER JOIN dbo.deliveryLine T1 ON T1.docId = T0.docId
		INNER JOIN dbo.salesOrder T2 ON T1.baseDocId = T2.docId AND T1.baseDocType = 'SO'
		INNER JOIN dbo.salesOrderLine T3 ON T1.baseDocId = T3.docId AND T1.baseLineNum = T3.lineNum
		LEFT OUTER JOIN dbo.batchDocument T4 ON T4.baseDocId = T1.docId AND T4.baseLineNum = T1.lineNum AND T4.baseDocType = 'DL'
		LEFT OUTER JOIN dbo.batch T5 ON T5.batchId = T4.batchId
		INNER JOIN dbo.item T6 ON T6.itemId = T1.itemId
		INNER JOIN dbo.businessPartner T7 ON T7.businessPartnerId = T0.businessPartnerId
	WHERE T6.itemCode NOT IN ('A', 'AV', 'BON', 'BOP', 'BTW', 'C', 'CLA', 'COM', 'D', 'E', 'GAR', 'K', 'N', 'NF', 'NFC', 'O', 'PR', 'RONGEN', 'S', 'SP', 'T', 'V')

