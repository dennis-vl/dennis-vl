/*
Run this script on:

        (local)\MSSQL.Portal_PROD_2014-05-19    -  This database will be modified

to synchronize it with:

        VLSRV905.Portal

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 19-8-2014 15:08:17

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'MS_DiagramPane1', 'SCHEMA', N'dbo', 'VIEW', N'vw_items', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_dropextendedproperty N'MS_DiagramPane2', 'SCHEMA', N'dbo', 'VIEW', N'vw_items', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_dropextendedproperty N'MS_DiagramPaneCount', 'SCHEMA', N'dbo', 'VIEW', N'vw_items', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[userRole]'
GO
ALTER TABLE [dbo].[userRole] DROP CONSTRAINT [FK_userRole_project]
ALTER TABLE [dbo].[userRole] DROP CONSTRAINT [FK_userRole_Users]
ALTER TABLE [dbo].[userRole] DROP CONSTRAINT [FK_userRole_applicationRole]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[userRole]'
GO
ALTER TABLE [dbo].[userRole] DROP CONSTRAINT [PK__userRole__2A167B39783FB9D5]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[vw_items]'
GO

ALTER VIEW [dbo].vw_items
AS
	-- ====================================================================================
	-- Name:    	vw_items
	-- Author:      Mark Kreukniet
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
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[businessPartnerDocuments]'
GO
CREATE TABLE [dbo].[businessPartnerDocuments]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[DOPLink] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[businessPartnerId] [int] NULL,
[description] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_declarationOfPermanceDocuments] on [dbo].[businessPartnerDocuments]'
GO
ALTER TABLE [dbo].[businessPartnerDocuments] ADD CONSTRAINT [PK_declarationOfPermanceDocuments] PRIMARY KEY CLUSTERED  ([ID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[vw_businessPartnerDocuments]'
GO
CREATE VIEW dbo.vw_businessPartnerDocuments
AS
SELECT     dbo.businessPartner.businessPartnerId, dbo.businessPartner.bpName, dbo.businessPartnerDocuments.DOPLink, dbo.businessPartnerDocuments.description
FROM         dbo.businessPartner INNER JOIN
                      dbo.businessPartnerDocuments ON dbo.businessPartner.businessPartnerId = dbo.businessPartnerDocuments.businessPartnerId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[businessPartnerSearch]'
GO
CREATE TABLE [dbo].[businessPartnerSearch]
(
[businessPartnerSearchID] [int] NOT NULL IDENTITY(1, 1),
[businessPartnerId] [int] NOT NULL,
[searchString] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_BussinessPartnerSearch] on [dbo].[businessPartnerSearch]'
GO
ALTER TABLE [dbo].[businessPartnerSearch] ADD CONSTRAINT [PK_BussinessPartnerSearch] PRIMARY KEY CLUSTERED  ([businessPartnerSearchID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_BussinessPartnerSearch] on [dbo].[businessPartnerSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_BussinessPartnerSearch] ON [dbo].[businessPartnerSearch] ([businessPartnerId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[vw_BusinessPartnerDocumentsBusinessPartnerBusinessPartnerSearch]'
GO
CREATE VIEW dbo.vw_BusinessPartnerDocumentsBusinessPartnerBusinessPartnerSearch
AS
SELECT     dbo.businessPartner.businessPartnerId, dbo.businessPartner.bpName, dbo.businessPartnerDocuments.DOPLink, 
                      dbo.businessPartnerDocuments.businessPartnerId AS Expr1, dbo.businessPartnerDocuments.description, dbo.businessPartnerSearch.businessPartnerSearchID, 
                      dbo.businessPartnerSearch.businessPartnerId AS Expr2, dbo.businessPartnerSearch.searchString
FROM         dbo.businessPartner INNER JOIN
                      dbo.businessPartnerDocuments ON dbo.businessPartner.businessPartnerId = dbo.businessPartnerDocuments.businessPartnerId INNER JOIN
                      dbo.businessPartnerSearch ON dbo.businessPartner.businessPartnerId = dbo.businessPartnerSearch.businessPartnerId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[userRole]'
GO
ALTER TABLE [dbo].[userRole] ADD
[userRoleId] [int] NOT NULL IDENTITY(1, 1)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[userRole] ALTER COLUMN [projectCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_userRole] on [dbo].[userRole]'
GO
ALTER TABLE [dbo].[userRole] ADD CONSTRAINT [PK_userRole] PRIMARY KEY CLUSTERED  ([userRoleId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[userRole]'
GO
ALTER TABLE [dbo].[userRole] ADD CONSTRAINT [FK_userRole_project] FOREIGN KEY ([projectCode]) REFERENCES [dbo].[project] ([projectCode])
ALTER TABLE [dbo].[userRole] ADD CONSTRAINT [FK_userRole_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([UserId])
ALTER TABLE [dbo].[userRole] ADD CONSTRAINT [FK_userRole_applicationRole] FOREIGN KEY ([roleCode]) REFERENCES [dbo].[applicationRole] ([roleCode])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[businessPartnerDocuments]'
GO
ALTER TABLE [dbo].[businessPartnerDocuments] ADD CONSTRAINT [FK_declarationOfPermanceDocuments_businessPartner] FOREIGN KEY ([businessPartnerId]) REFERENCES [dbo].[businessPartner] ([businessPartnerId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[businessPartnerSearch]'
GO
ALTER TABLE [dbo].[businessPartnerSearch] ADD CONSTRAINT [FK_BussinessPartnerSearch_businessPartner] FOREIGN KEY ([businessPartnerId]) REFERENCES [dbo].[businessPartner] ([businessPartnerId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "businessPartner"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "businessPartnerDocuments"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 125
               Right = 456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "businessPartnerSearch"
            Begin Extent = 
               Top = 6
               Left = 494
               Bottom = 110
               Right = 704
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'vw_BusinessPartnerDocumentsBusinessPartnerBusinessPartnerSearch', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'vw_BusinessPartnerDocumentsBusinessPartnerBusinessPartnerSearch', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "businessPartner"
            Begin Extent = 
               Top = 105
               Left = 128
               Bottom = 224
               Right = 332
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "businessPartnerDocuments"
            Begin Extent = 
               Top = 96
               Left = 388
               Bottom = 215
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'vw_businessPartnerDocuments', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'vw_businessPartnerDocuments', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO



INSERT INTO dbo.application
        ( applicationCode ,
          applicationName
        )
VALUES  ( N'DOP' , -- applicationCode - nvarchar(50)
          'Declaration Of Performance'  -- applicationName - varchar(255)
        )

INSERT INTO dbo.application
        ( applicationCode ,
          applicationName
        )
VALUES  ( N'CERT' , -- applicationCode - nvarchar(50)
          'Certificates'  -- applicationName - varchar(255)
        )
        
INSERT INTO dbo.applicationRole
SELECT 'CERT_ADMIN', 'CERT', 'CERT Admin'

INSERT INTO dbo.applicationRole
SELECT 'CERT_USER', 'CERT', 'CERT User'

INSERT INTO dbo.applicationRole
SELECT 'DOP_ADMIN', 'DOP', 'DOP Admin'

INSERT INTO dbo.applicationRole
SELECT 'DOP_USER', 'DOP', 'DOP User'


INSERT INTO dbo.userRole
SELECT T0.userId, 'CERT_USER', NULL 
FROM dbo.Users T0
	INNER JOIN dbo.Memberships T1 ON T1.UserId = T0.UserId
	
INSERT INTO dbo.userRole
SELECT T0.userId, 'DOP_USER', NULL 
FROM dbo.Users T0
	INNER JOIN dbo.Memberships T1 ON T1.UserId = T0.UserId
	INNER JOIN dbo.UsersInRoles T2 ON T0.UserId = T2.UserId
WHERE T2.RoleId = 'F589EB71-BD5E-4C92-81BE-840BD98132E7'