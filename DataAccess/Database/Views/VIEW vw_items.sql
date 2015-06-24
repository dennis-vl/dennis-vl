IF EXISTS (SELECT name FROM sysobjects
					 WHERE name = 'vw_items' AND type = 'V') 
	DROP VIEW [dbo].vw_items
GO

CREATE VIEW [dbo].vw_items
AS
	-- ====================================================================================
	-- Name:    		vw_items
	-- Author:      William van Aert
	-- 
	-- Version:     20140409.01
	--
	-- Description: View for items
	-- ====================================================================================

	SELECT     T0.itemId, T0.itemCode, T0.description, T0.size, T0.weight, ISNULL(T0.timeStampLastUpdate, T0.timeStampCreated) AS timeStampLastUpdate, T11.outsideDiameter1, T11.outsideDiameter2, T11.outsideDiameter3, T11.wallThickness1, 
                      T11.wallThickness2, T11.type, T11.specification, T11.length, T11.treatmentHeat, T11.treatmentSurface, T11.ends, T11.cdi, T11.supplier, T11.other, T11.certificates, 
                      T1.itemGroupName, ISNULL(SUM(T2.stockAvailable), 0) AS stockAvailable, SUM(T2.stockOnHand) AS stockOnHand, ISNULL(T3.price, 0) AS mAvPrice, ISNULL(T4.price, 
                      0) AS grossPrice, ISNULL(T5.companyName, T8.companyName) AS companyName, ISNULL(T5.companyCode, T8.companyCode) AS companyCode, ISNULL(T10.openQuantity, 0) AS purchaseOpenQuantity
    FROM         dbo.item AS T0 LEFT OUTER JOIN
                      dbo.itemVLunar AS T11 ON T0.itemId = T11.itemId INNER JOIN
                      dbo.itemGroup AS T1 ON T0.itemGroupCode = T1.itemGroupCode LEFT OUTER JOIN
                      dbo.company AS T5 ON T0.companyCode = T5.companyCode LEFT OUTER JOIN
                      dbo.companyGroup AS T6 ON T0.companyGroupCode = T6.companyGroupCode LEFT OUTER JOIN
                      dbo.companyGroupForItems AS T7 ON T6.companyGroupCode = T7.companyGroupCode LEFT OUTER JOIN
                      dbo.company AS T8 ON T7.companyCode = T8.companyCode LEFT OUTER JOIN
                      dbo.stock AS T2 ON T0.itemId = T2.itemId LEFT OUTER JOIN
                      dbo.warehouse AS T9 ON T2.warehouseId = T9.warehouseId AND T9.companyCode = ISNULL(T5.companyName, T8.companyName) LEFT OUTER JOIN
                      dbo.priceItem AS T3 ON T0.itemId = T3.itemId AND T3.companyCode = ISNULL(T5.companyCode, T8.companyCode) AND T3.priceListCode = 'mAv' LEFT OUTER JOIN
                      dbo.priceItem AS T4 ON T0.itemId = T4.itemId AND T4.companyCode = ISNULL(T5.companyCode, T8.companyCode) AND T4.priceListCode = 'gross' LEFT OUTER JOIN
                          (SELECT     T20.companyCode, T21.itemId, SUM(T21.openQuantity) AS openQuantity
                            FROM          dbo.purchaseOrder AS T20 INNER JOIN
                                                   dbo.purchaseOrderLine AS T21 ON T20.docId = T21.docId
                            GROUP BY T20.companyCode, T21.itemId) AS T10 ON T4.companyCode = T10.companyCode AND T4.itemId = T10.itemId
    GROUP BY T0.itemId, T0.itemCode, T1.itemGroupName, T5.companyName, T8.companyName, T5.companyCode, T8.companyCode, T0.timeStampLastUpdate, T0.timeStampCreated, T3.price, T4.price, T0.description, T0.size, T0.weight, T11.outsideDiameter1, 
                      T11.outsideDiameter2, T11.outsideDiameter3, T11.wallThickness1, T11.wallThickness2, T11.type, T11.specification, T11.length, T11.treatmentHeat, 
                      T11.treatmentSurface, T11.ends, T11.cdi, T11.supplier, T11.other, T11.certificates, T10.openQuantity