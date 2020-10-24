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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ForecastGroupToTimePeriodToProfile]') AND type in (N'U'))
DROP TABLE [Mapping].[ForecastGroupToTimePeriodToProfile]
GO
CREATE TABLE [Mapping].[ForecastGroupToTimePeriodToProfile]
	(
	ForecastGroupToTimePeriodToProfileId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastGroupToTimePeriodId BIGINT NOT NULL,
	ProfileId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	PK_ForecastGroupToTimePeriodToProfile PRIMARY KEY CLUSTERED 
	(
	ForecastGroupToTimePeriodToProfileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfile_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfile_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	DF_ForecastGroupToTimePeriodToProfile_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfile_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfile].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfile', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfile_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfile_ForecastGroupToTimePeriodId FOREIGN KEY
	(
	ForecastGroupToTimePeriodId
	) REFERENCES [Mapping].[ForecastGroupToTimePeriod]
	(
	ForecastGroupToTimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfile].ForecastGroupToTimePeriodId to [Mapping].[ForecastGroupToTimePeriod].ForecastGroupToTimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfile', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfile_ForecastGroupToTimePeriodId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfile_ProfileId FOREIGN KEY
	(
	ProfileId
	) REFERENCES [DemandForecast].[Profile]
	(
	ProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfile].ProfileId to [DemandForecast].[Profile].ProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfile', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfile_ProfileId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] ADD CONSTRAINT
	FK_ForecastGroupToTimePeriodToProfile_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ForecastGroupToTimePeriodToProfile].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ForecastGroupToTimePeriodToProfile', N'CONSTRAINT', N'FK_ForecastGroupToTimePeriodToProfile_SourceId'
GO
ALTER TABLE [Mapping].[ForecastGroupToTimePeriodToProfile] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
