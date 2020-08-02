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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[GridSupplyPointAttribute]') AND type in (N'U'))
DROP TABLE [Information].[GridSupplyPointAttribute]
GO
CREATE TABLE [Information].[GridSupplyPointAttribute]
	(
	GridSupplyPointAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	GridSupplyPointAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	PK_GridSupplyPointAttribute PRIMARY KEY CLUSTERED 
	(
	GridSupplyPointAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	DF_GridSupplyPointAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	DF_GridSupplyPointAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	DF_GridSupplyPointAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	FK_GridSupplyPointAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointAttribute', N'CONSTRAINT', N'FK_GridSupplyPointAttribute_CreatedByUserId'
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] ADD CONSTRAINT
	FK_GridSupplyPointAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointAttribute', N'CONSTRAINT', N'FK_GridSupplyPointAttribute_SourceId'
GO
ALTER TABLE [Information].[GridSupplyPointAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
