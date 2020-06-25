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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[LocalDistributionZoneAttribute]') AND type in (N'U'))
DROP TABLE [Information].[LocalDistributionZoneAttribute]
GO
CREATE TABLE [Information].[LocalDistributionZoneAttribute]
	(
	LocalDistributionZoneAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LocalDistributionZoneAttributeDescription VARCHAR(200) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	PK_LocalDistributionZoneAttribute PRIMARY KEY CLUSTERED 
	(
	LocalDistributionZoneAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	DF_LocalDistributionZoneAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	DF_LocalDistributionZoneAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	DF_LocalDistributionZoneAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	FK_LocalDistributionZoneAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneAttribute', N'CONSTRAINT', N'FK_LocalDistributionZoneAttribute_CreatedByUserId'
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] ADD CONSTRAINT
	FK_LocalDistributionZoneAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneAttribute', N'CONSTRAINT', N'FK_LocalDistributionZoneAttribute_SourceId'
GO
ALTER TABLE [Information].[LocalDistributionZoneAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
