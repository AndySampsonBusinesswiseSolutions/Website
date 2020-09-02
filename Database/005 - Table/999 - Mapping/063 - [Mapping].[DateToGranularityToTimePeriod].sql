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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToGranularityToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToGranularityToTimePeriod]
GO
CREATE TABLE [Mapping].[DateToGranularityToTimePeriod]
	(
	DateToGranularityToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	GranularityToTimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	PK_DateToGranularityToTimePeriod PRIMARY KEY CLUSTERED 
	(
	DateToGranularityToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	DF_DateToGranularityToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	DF_DateToGranularityToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	DF_DateToGranularityToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	FK_DateToGranularityToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToGranularityToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToGranularityToTimePeriod', N'CONSTRAINT', N'FK_DateToGranularityToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	FK_DateToGranularityToTimePeriod_GranularityToTimePeriodId FOREIGN KEY
	(
	GranularityToTimePeriodId
	) REFERENCES [Mapping].[GranularityToTimePeriod]
	(
	GranularityToTimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToGranularityToTimePeriod].GranularityToTimePeriodId to [Mapping].[GranularityToTimePeriod].GranularityToTimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToGranularityToTimePeriod', N'CONSTRAINT', N'FK_DateToGranularityToTimePeriod_GranularityToTimePeriodId'
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	FK_DateToGranularityToTimePeriod_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToGranularityToTimePeriod].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToGranularityToTimePeriod', N'CONSTRAINT', N'FK_DateToGranularityToTimePeriod_DateId'
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] ADD CONSTRAINT
	FK_DateToGranularityToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToGranularityToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToGranularityToTimePeriod', N'CONSTRAINT', N'FK_DateToGranularityToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[DateToGranularityToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
