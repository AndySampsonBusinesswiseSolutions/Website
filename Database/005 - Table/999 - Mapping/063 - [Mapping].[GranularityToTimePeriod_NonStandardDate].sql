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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[GranularityToTimePeriod_NonStandardDate]') AND type in (N'U'))
DROP TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate]
GO
CREATE TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate]
	(
	GranularityToTimePeriod_NonStandardDateId BIGINT IDENTITY(1,1) NOT NULL,
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
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	PK_GranularityToTimePeriod_NonStandardDate PRIMARY KEY CLUSTERED 
	(
	GranularityToTimePeriod_NonStandardDateId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	DF_GranularityToTimePeriod_NonStandardDate_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	DF_GranularityToTimePeriod_NonStandardDate_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	DF_GranularityToTimePeriod_NonStandardDate_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	FK_GranularityToTimePeriod_NonStandardDate_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_NonStandardDate].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_NonStandardDate', N'CONSTRAINT', N'FK_GranularityToTimePeriod_NonStandardDate_CreatedByUserId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	FK_GranularityToTimePeriod_NonStandardDate_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_NonStandardDate].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_NonStandardDate', N'CONSTRAINT', N'FK_GranularityToTimePeriod_NonStandardDate_TimePeriodId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	FK_GranularityToTimePeriod_NonStandardDate_GranularityId FOREIGN KEY
	(
	GranularityId
	) REFERENCES [Information].[Granularity]
	(
	GranularityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_NonStandardDate].GranularityId to [Information].[Granularity].GranularityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_NonStandardDate', N'CONSTRAINT', N'FK_GranularityToTimePeriod_NonStandardDate_GranularityId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	FK_GranularityToTimePeriod_NonStandardDate_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_NonStandardDate].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_NonStandardDate', N'CONSTRAINT', N'FK_GranularityToTimePeriod_NonStandardDate_DateId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] ADD CONSTRAINT
	FK_GranularityToTimePeriod_NonStandardDate_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GranularityToTimePeriod_NonStandardDate].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GranularityToTimePeriod_NonStandardDate', N'CONSTRAINT', N'FK_GranularityToTimePeriod_NonStandardDate_SourceId'
GO
ALTER TABLE [Mapping].[GranularityToTimePeriod_NonStandardDate] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
