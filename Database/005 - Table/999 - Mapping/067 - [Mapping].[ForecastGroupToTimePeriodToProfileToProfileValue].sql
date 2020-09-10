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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]') AND type in (N'U'))
DROP TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]
GO
CREATE TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]
	(
	ForecastGroupToTimePeriodToProfileToProfileValueId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastGroupToTimePeriodToProfileId BIGINT NOT NULL,
	ProfileValueId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	PK_ForecastGroupToTimePeriodToProfileToProfileValue PRIMARY KEY CLUSTERED 
	(
	ForecastGroupToTimePeriodToProfileToProfileValueId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileToProfileValue_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileToProfileValue_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfileToProfileValue_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileToProfileValue_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileToProfileValue_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileToProfileValue_ForecastGroupToTimePeriodToProfileId FOREIGN KEY
	(
	ForecastGroupToTimePeriodToProfileId
	) REFERENCES [Mapping].[ForecastGroupToTimePeriodToProfile]
	(
	ForecastGroupToTimePeriodToProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue].ForecastGroupToTimePeriodToProfileId to [Mapping].[ForecastGroupToTimePeriodToProfile].ForecastGroupToTimePeriodToProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileToProfileValue_ForecastGroupToTimePeriodToProfileId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileToProfileValue_ProfileValueId FOREIGN KEY
	(
	ProfileValueId
	) REFERENCES [DemandForecast].[ProfileValue]
	(
	ProfileValueId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue].ProfileId to [DemandForecast].[ProfileValue].ProfileValueId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileToProfileValue_ProfileValueId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfileToProfileValue_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfileToProfileValue', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfileToProfileValue_SourceId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
