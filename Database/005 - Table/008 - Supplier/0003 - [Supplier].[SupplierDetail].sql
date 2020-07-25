USE [EMaaS]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Supplier].[SupplierDetail]') AND type in (N'U'))
DROP TABLE [Supplier].[SupplierDetail]
GO
CREATE TABLE [Supplier].[SupplierDetail]
	(
	SupplierDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	SupplierId BIGINT NOT NULL,
	SupplierAttributeId BIGINT NOT NULL,
	SupplierDetailDescription VARCHAR(255) NOT NULL
	)  ON [Supplier]
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	PK_SupplierDetail PRIMARY KEY CLUSTERED 
	(
	SupplierDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supplier]

GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	DF_SupplierDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	DF_SupplierDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	DF_SupplierDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	FK_SupplierDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[SupplierDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'SupplierDetail', N'CONSTRAINT', N'FK_SupplierDetail_CreatedByUserId'
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	FK_SupplierDetail_SupplierId FOREIGN KEY
	(
	SupplierId
	) REFERENCES [Supplier].[Supplier]
	(
	SupplierId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[SupplierDetail].SupplierId to [Supplier].[Supplier].SupplierId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'SupplierDetail', N'CONSTRAINT', N'FK_SupplierDetail_SupplierId'
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	FK_SupplierDetail_SupplierAttributeId FOREIGN KEY
	(
	SupplierAttributeId
	) REFERENCES [Supplier].[SupplierAttribute]
	(
	SupplierAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[SupplierDetail].SupplierAttributeId to [Supplier].[SupplierAttribute].SupplierAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'SupplierDetail', N'CONSTRAINT', N'FK_SupplierDetail_SupplierAttributeId'
GO
ALTER TABLE [Supplier].[SupplierDetail] ADD CONSTRAINT
	FK_SupplierDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[SupplierDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'SupplierDetail', N'CONSTRAINT', N'FK_SupplierDetail_SourceId'
GO
ALTER TABLE [Supplier].[SupplierDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
