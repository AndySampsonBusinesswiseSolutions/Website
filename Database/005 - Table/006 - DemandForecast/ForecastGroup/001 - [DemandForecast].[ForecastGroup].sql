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
USE [EMaaS]
GO
CREATE TABLE [DemandForecast].[ForecastGroup]
	(
	ForecastGroupId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastGroupDescription VARCHAR(250) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	DF_ForecastGroup_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	DF_ForecastGroup_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	DF_ForecastGroup_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	PK_ForecastGroup PRIMARY KEY CLUSTERED 
	(
	ForecastGroupId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	FK_ForecastGroup_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastGroup].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastGroup', N'CONSTRAINT', N'FK_ForecastGroup_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ForecastGroup] ADD CONSTRAINT
	FK_ForecastGroup_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastGroup].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastGroup', N'CONSTRAINT', N'FK_ForecastGroup_SourceId'
GO
ALTER TABLE [DemandForecast].[ForecastGroup] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
