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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ForecastGroupToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[ForecastGroupToTimePeriod]
GO
CREATE TABLE [Mapping].[ForecastGroupToTimePeriod]
	(
	ForecastGroupToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastGroupId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	PK_ForecastGroupToTimePeriod PRIMARY KEY CLUSTERED 
	(
	ForecastGroupToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriod', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriod_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriod].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriod', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriod_TimePeriodId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriod_ForecastGroupId FOREIGN KEY
	(
	ForecastGroupId
	) REFERENCES [DemandForecast].[ForecastGroup]
	(
	ForecastGroupId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriod].ForecastGroupId to [DemandForecast].[ForecastGroup].ForecastGroupId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriod', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriod_ForecastGroupId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriod', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
