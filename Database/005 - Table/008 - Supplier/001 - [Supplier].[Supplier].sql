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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Supplier].[Supplier]') AND type in (N'U'))
DROP TABLE [Supplier].[Supplier]
GO
CREATE TABLE [Supplier].[Supplier]
	(
	SupplierId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	SupplierGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [Supplier]
GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	PK_Supplier PRIMARY KEY CLUSTERED 
	(
	SupplierId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supplier]

GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	DF_Supplier_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	DF_Supplier_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	DF_Supplier_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	FK_Supplier_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[Supplier].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'Supplier', N'CONSTRAINT', N'FK_Supplier_CreatedByUserId'
GO
ALTER TABLE [Supplier].[Supplier] ADD CONSTRAINT
	FK_Supplier_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Supplier].[Supplier].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Supplier', N'TABLE', N'Supplier', N'CONSTRAINT', N'FK_Supplier_SourceId'
GO
ALTER TABLE [Supplier].[Supplier] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
