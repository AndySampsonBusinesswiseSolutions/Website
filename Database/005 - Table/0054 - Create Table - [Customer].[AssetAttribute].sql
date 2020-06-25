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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[AssetAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[AssetAttribute]
GO
CREATE TABLE [Customer].[AssetAttribute]
	(
	AssetAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	AssetAttributeDescription VARCHAR(200) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	PK_AssetAttribute PRIMARY KEY CLUSTERED 
	(
	AssetAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	DF_AssetAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	DF_AssetAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	DF_AssetAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	FK_AssetAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetAttribute', N'CONSTRAINT', N'FK_AssetAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[AssetAttribute] ADD CONSTRAINT
	FK_AssetAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetAttribute', N'CONSTRAINT', N'FK_AssetAttribute_SourceId'
GO
ALTER TABLE [Customer].[AssetAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
