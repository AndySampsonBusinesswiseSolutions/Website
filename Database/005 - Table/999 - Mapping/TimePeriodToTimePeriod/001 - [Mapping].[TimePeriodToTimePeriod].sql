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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[TimePeriodToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[TimePeriodToTimePeriod]
GO
CREATE TABLE [Mapping].[TimePeriodToTimePeriod]
	(
	TimePeriodToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL,
	MappedTimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	PK_TimePeriodToTimePeriod PRIMARY KEY CLUSTERED 
	(
	TimePeriodToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	DF_TimePeriodToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	DF_TimePeriodToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	DF_TimePeriodToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	FK_TimePeriodToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TimePeriodToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TimePeriodToTimePeriod', N'CONSTRAINT', N'FK_TimePeriodToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	FK_TimePeriodToTimePeriod_MappedTimePeriodId FOREIGN KEY
	(
	MappedTimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TimePeriodToTimePeriod].MappedTimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TimePeriodToTimePeriod', N'CONSTRAINT', N'FK_TimePeriodToTimePeriod_MappedTimePeriodId'
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	FK_TimePeriodToTimePeriod_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TimePeriodToTimePeriod].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TimePeriodToTimePeriod', N'CONSTRAINT', N'FK_TimePeriodToTimePeriod_TimePeriodId'
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] ADD CONSTRAINT
	FK_TimePeriodToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TimePeriodToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TimePeriodToTimePeriod', N'CONSTRAINT', N'FK_TimePeriodToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[TimePeriodToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
