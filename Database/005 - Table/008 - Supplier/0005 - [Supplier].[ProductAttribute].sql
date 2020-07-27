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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Supplier].[ProductAttribute]') AND type in (N'U'))
DROP TABLE [Supplier].[ProductAttribute]
GO
CREATE TABLE [Supplier].[ProductAttribute]
	(
	ProductAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProductAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Supplier]
GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	PK_ProductAttribute PRIMARY KEY CLUSTERED 
	(
	ProductAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supplier]

GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	DF_ProductAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	DF_ProductAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	DF_ProductAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	FK_ProductAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductAttribute', N'CONSTRAINT', N'FK_ProductAttribute_CreatedByUserId'
GO
ALTER TABLE [Supplier].[ProductAttribute] ADD CONSTRAINT
	FK_ProductAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[ProductAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'ProductAttribute', N'CONSTRAINT', N'FK_ProductAttribute_SourceId'
GO
ALTER TABLE [Supplier].[ProductAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
