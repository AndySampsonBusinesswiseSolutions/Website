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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[GranularityToTimePeriod_DateOverride]') AND type in (N'U'))
DROP TABLE [Mapping].[GranularityToTimePeriod_DateOverride]
GO
CREATE TABLE [Mapping].[GranularityToTimePeriod_DateOverride]
	(
	GranularityToTimePeriod_DateOverrideId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	GranularityId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL,
	DateId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	PK_GranularityToTimePeriod_DateOverride PRIMARY KEY CLUSTERED 
	(
	GranularityToTimePeriod_DateOverrideId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	DF_GranularityToTimePeriod_DateOverride_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	DF_GranularityToTimePeriod_DateOverride_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	DF_GranularityToTimePeriod_DateOverride_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	FK_GranularityToTimePeriod_DateOverride_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_DateOverride].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_DateOverride', N'CONSTRAINT', N'FK_GranularityToTimePeriod_DateOverride_CreatedByUserId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	FK_GranularityToTimePeriod_DateOverride_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_DateOverride].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_DateOverride', N'CONSTRAINT', N'FK_GranularityToTimePeriod_DateOverride_TimePeriodId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	FK_GranularityToTimePeriod_DateOverride_GranularityId FOREIGN KEY
	(
	GranularityId
	) REFERENCES [Information].[Granularity]
	(
	GranularityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_DateOverride].GranularityId to [Information].[Granularity].GranularityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_DateOverride', N'CONSTRAINT', N'FK_GranularityToTimePeriod_DateOverride_GranularityId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	FK_GranularityToTimePeriod_DateOverride_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_DateOverride].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_DateOverride', N'CONSTRAINT', N'FK_GranularityToTimePeriod_DateOverride_DateId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] ADD CONSTRAINT
	FK_GranularityToTimePeriod_DateOverride_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_DateOverride].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_DateOverride', N'CONSTRAINT', N'FK_GranularityToTimePeriod_DateOverride_SourceId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_DateOverride] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
