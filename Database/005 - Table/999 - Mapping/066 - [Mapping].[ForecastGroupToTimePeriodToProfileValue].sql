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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ForecastGroupToTimePeriodToProfileValue]') AND type in (N'U'))
DROP TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue]
GO
CREATE TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue]
	(
	ForecastGroupToTimePeriodToProfileValueId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastGroupToTimePeriodId BIGINT NOT NULL,
	ProfileValueId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	PK_ForecastGroupToTimePeriodToProfileValue PRIMARY KEY CLUSTERED 
	(
	ForecastGroupToTimePeriodToProfileValueId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileValue_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileValue_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileValue_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileValue_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileValue].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileValue_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileValue_ForecastGroupToTimePeriodId FOREIGN KEY
	(
	ForecastGroupToTimePeriodId
	) REFERENCES [Mapping].[ForecastGroupToTimePeriod]
	(
	ForecastGroupToTimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileValue].ForecastGroupToTimePeriodId to [Mapping].[ForecastGroupToTimePeriod].ForecastGroupToTimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileValue_ForecastGroupToTimePeriodId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileValue_ProfileValueId FOREIGN KEY
	(
	ProfileValueId
	) REFERENCES [DemandForecast].[ProfileValue]
	(
	ProfileValueId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileValue].ProfileValueId to [DemandForecast].[ProfileValue].ProfileValueId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileValue_ProfileValueId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileValue_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileValue].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileValue_SourceId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileValue] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
