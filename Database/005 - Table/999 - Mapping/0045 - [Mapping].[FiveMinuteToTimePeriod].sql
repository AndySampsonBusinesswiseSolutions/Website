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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[FiveMinuteToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[FiveMinuteToTimePeriod]
GO
CREATE TABLE [Mapping].[FiveMinuteToTimePeriod]
	(
	FiveMinuteToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FiveMinuteId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	PK_FiveMinuteToTimePeriod PRIMARY KEY CLUSTERED 
	(
	FiveMinuteToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	DF_FiveMinuteToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	DF_FiveMinuteToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	DF_FiveMinuteToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	FK_FiveMinuteToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FiveMinuteToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FiveMinuteToTimePeriod', N'CONSTRAINT', N'FK_FiveMinuteToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	FK_FiveMinuteToTimePeriod_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FiveMinuteToTimePeriod].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FiveMinuteToTimePeriod', N'CONSTRAINT', N'FK_FiveMinuteToTimePeriod_TimePeriodId'
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	FK_FiveMinuteToTimePeriod_FiveMinuteId FOREIGN KEY
	(
	FiveMinuteId
	) REFERENCES [Information].[FiveMinute]
	(
	FiveMinuteId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FiveMinuteToTimePeriod].FiveMinuteId to [Information].[FiveMinute].FiveMinuteId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FiveMinuteToTimePeriod', N'CONSTRAINT', N'FK_FiveMinuteToTimePeriod_FiveMinuteId'
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] ADD CONSTRAINT
	FK_FiveMinuteToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FiveMinuteToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FiveMinuteToTimePeriod', N'CONSTRAINT', N'FK_FiveMinuteToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[FiveMinuteToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
