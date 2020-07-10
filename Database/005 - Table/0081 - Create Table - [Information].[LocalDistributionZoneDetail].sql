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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[LocalDistributionZoneDetail]') AND type in (N'U'))
DROP TABLE [Information].[LocalDistributionZoneDetail]
GO
CREATE TABLE [Information].[LocalDistributionZoneDetail]
	(
	LocalDistributionZoneDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LocalDistributionZoneId BIGINT NOT NULL,
	LocalDistributionZoneAttributeId BIGINT NOT NULL,
	LocalDistributionZoneDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	PK_LocalDistributionZoneDetail PRIMARY KEY CLUSTERED 
	(
	LocalDistributionZoneDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	DF_LocalDistributionZoneDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	DF_LocalDistributionZoneDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	DF_LocalDistributionZoneDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	FK_LocalDistributionZoneDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneDetail', N'CONSTRAINT', N'FK_LocalDistributionZoneDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	FK_LocalDistributionZoneDetail_LocalDistributionZoneId FOREIGN KEY
	(
	LocalDistributionZoneId
	) REFERENCES [Information].[LocalDistributionZone]
	(
	LocalDistributionZoneId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneDetail].LocalDistributionZoneId to [Information].[LocalDistributionZone].LocalDistributionZoneId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneDetail', N'CONSTRAINT', N'FK_LocalDistributionZoneDetail_LocalDistributionZoneId'
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	FK_LocalDistributionZoneDetail_LocalDistributionZoneAttributeId FOREIGN KEY
	(
	LocalDistributionZoneAttributeId
	) REFERENCES [Information].[LocalDistributionZoneAttribute]
	(
	LocalDistributionZoneAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneDetail].LocalDistributionZoneAttributeId to [Information].[LocalDistributionZoneAttribute].LocalDistributionZoneAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneDetail', N'CONSTRAINT', N'FK_LocalDistributionZoneDetail_LocalDistributionZoneAttributeId'
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] ADD CONSTRAINT
	FK_LocalDistributionZoneDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LocalDistributionZoneDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LocalDistributionZoneDetail', N'CONSTRAINT', N'FK_LocalDistributionZoneDetail_SourceId'
GO
ALTER TABLE [Information].[LocalDistributionZoneDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
