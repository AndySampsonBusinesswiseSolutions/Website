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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Supplier].[ProductDetail]') AND type in (N'U'))
DROP TABLE [Supplier].[ProductDetail]
GO
CREATE TABLE [Supplier].[ProductDetail]
	(
	ProductDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	ProductAttributeId BIGINT NOT NULL,
	ProductDetailDescription VARCHAR(255) NOT NULL
	)  ON [Supplier]
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	PK_ProductDetail PRIMARY KEY CLUSTERED 
	(
	ProductDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supplier]

GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	DF_ProductDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	DF_ProductDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	DF_ProductDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	FK_ProductDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductDetail', N'CONSTRAINT', N'FK_ProductDetail_CreatedByUserId'
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	FK_ProductDetail_ProductId FOREIGN KEY
	(
	ProductId
	) REFERENCES [Supplier].[Product]
	(
	ProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductDetail].ProductId to [Supplier].[Supplier].ProductId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductDetail', N'CONSTRAINT', N'FK_ProductDetail_ProductId'
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	FK_ProductDetail_ProductAttributeId FOREIGN KEY
	(
	ProductAttributeId
	) REFERENCES [Supplier].[ProductAttribute]
	(
	ProductAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductDetail].ProductAttributeId to [Supplier].[ProductAttribute].ProductAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductDetail', N'CONSTRAINT', N'FK_ProductDetail_ProductAttributeId'
GO
ALTER TABLE [Supplier].[ProductDetail] ADD CONSTRAINT
	FK_ProductDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductDetail', N'CONSTRAINT', N'FK_ProductDetail_SourceId'
GO
ALTER TABLE [Supplier].[ProductDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
