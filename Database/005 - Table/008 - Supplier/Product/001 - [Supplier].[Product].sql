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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Supplier].[Product]') AND type in (N'U'))
DROP TABLE [Supplier].[Product]
GO
CREATE TABLE [Supplier].[Product]
	(
	ProductId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProductGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [Supplier]
GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	PK_Product PRIMARY KEY CLUSTERED 
	(
	ProductId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supplier]

GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	DF_Product_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	DF_Product_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	DF_Product_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	FK_Product_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[Product].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'Product', N'CONSTRAINT', N'FK_Product_CreatedByUserId'
GO
ALTER TABLE [Supplier].[Product] ADD CONSTRAINT
	FK_Product_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[Product].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'Product', N'CONSTRAINT', N'FK_Product_SourceId'
GO
ALTER TABLE [Supplier].[Product] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
