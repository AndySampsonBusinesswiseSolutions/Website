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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[HalfHourToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[HalfHourToTimePeriod]
GO
CREATE TABLE [Mapping].[HalfHourToTimePeriod]
	(
	HalfHourToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	HalfHourId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	PK_HalfHourToTimePeriod PRIMARY KEY CLUSTERED 
	(
	HalfHourToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	DF_HalfHourToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	DF_HalfHourToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	DF_HalfHourToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	FK_HalfHourToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[HalfHourToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'HalfHourToTimePeriod', N'CONSTRAINT', N'FK_HalfHourToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	FK_HalfHourToTimePeriod_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[HalfHourToTimePeriod].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'HalfHourToTimePeriod', N'CONSTRAINT', N'FK_HalfHourToTimePeriod_TimePeriodId'
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	FK_HalfHourToTimePeriod_HalfHourId FOREIGN KEY
	(
	HalfHourId
	) REFERENCES [Information].[HalfHour]
	(
	HalfHourId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[HalfHourToTimePeriod].HalfHourId to [Information].[HalfHour].HalfHourId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'HalfHourToTimePeriod', N'CONSTRAINT', N'FK_HalfHourToTimePeriod_HalfHourId'
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] ADD CONSTRAINT
	FK_HalfHourToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[HalfHourToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'HalfHourToTimePeriod', N'CONSTRAINT', N'FK_HalfHourToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[HalfHourToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
