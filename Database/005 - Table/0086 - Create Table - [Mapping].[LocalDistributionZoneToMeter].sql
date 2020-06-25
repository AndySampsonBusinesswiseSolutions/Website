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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[LocalDistributionZoneToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[LocalDistributionZoneToMeter]
GO
CREATE TABLE [Mapping].[LocalDistributionZoneToMeter]
	(
	LocalDistributionZoneToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LocalDistributionZoneId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	PK_LocalDistributionZoneToMeter PRIMARY KEY CLUSTERED 
	(
	LocalDistributionZoneToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	DF_LocalDistributionZoneToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	DF_LocalDistributionZoneToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	DF_LocalDistributionZoneToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	FK_LocalDistributionZoneToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LocalDistributionZoneToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LocalDistributionZoneToMeter', N'CONSTRAINT', N'FK_LocalDistributionZoneToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	FK_LocalDistributionZoneToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LocalDistributionZoneToMeter].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LocalDistributionZoneToMeter', N'CONSTRAINT', N'FK_LocalDistributionZoneToMeter_MeterId'
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	FK_LocalDistributionZoneToMeter_LocalDistributionZoneId FOREIGN KEY
	(
	LocalDistributionZoneId
	) REFERENCES [Information].[LocalDistributionZone]
	(
	LocalDistributionZoneId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LocalDistributionZoneToMeter].LocalDistributionZoneId to [Information].[LocalDistributionZone].LocalDistributionZoneId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LocalDistributionZoneToMeter', N'CONSTRAINT', N'FK_LocalDistributionZoneToMeter_LocalDistributionZoneId'
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] ADD CONSTRAINT
	FK_LocalDistributionZoneToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LocalDistributionZoneToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LocalDistributionZoneToMeter', N'CONSTRAINT', N'FK_LocalDistributionZoneToMeter_SourceId'
GO
ALTER TABLE [Mapping].[LocalDistributionZoneToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
