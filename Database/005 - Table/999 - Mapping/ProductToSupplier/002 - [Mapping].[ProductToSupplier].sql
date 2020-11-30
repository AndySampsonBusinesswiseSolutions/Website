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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ProductToSupplier]') AND type in (N'U'))
DROP TABLE [Mapping].[ProductToSupplier]
GO
CREATE TABLE [Mapping].[ProductToSupplier]
	(
	ProductToSupplierId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	SupplierId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	PK_ProductToSupplier PRIMARY KEY CLUSTERED 
	(
	ProductToSupplierId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	DF_ProductToSupplier_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	DF_ProductToSupplier_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	DF_ProductToSupplier_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	FK_ProductToSupplier_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProductToSupplier].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProductToSupplier', N'CONSTRAINT', N'FK_ProductToSupplier_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	FK_ProductToSupplier_SupplierId FOREIGN KEY
	(
	SupplierId
	) REFERENCES [Supplier].[Supplier]
	(
	SupplierId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProductToSupplier].SupplierId to [Supplier].[Supplier].SupplierId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProductToSupplier', N'CONSTRAINT', N'FK_ProductToSupplier_SupplierId'
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	FK_ProductToSupplier_ProductId FOREIGN KEY
	(
	ProductId
	) REFERENCES [Supplier].[Product]
	(
	ProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProductToSupplier].ProductId to [Supplier].[Supplier].ProductId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProductToSupplier', N'CONSTRAINT', N'FK_ProductToSupplier_ProductId'
GO
ALTER TABLE [Mapping].[ProductToSupplier] ADD CONSTRAINT
	FK_ProductToSupplier_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProductToSupplier].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProductToSupplier', N'CONSTRAINT', N'FK_ProductToSupplier_SourceId'
GO
ALTER TABLE [Mapping].[ProductToSupplier] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
