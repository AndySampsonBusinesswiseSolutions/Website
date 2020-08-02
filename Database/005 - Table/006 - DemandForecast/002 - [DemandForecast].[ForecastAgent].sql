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
CREATE TABLE [DemandForecast].[ForecastAgent]
	(
	ForecastAgentId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastAgent VARCHAR(250) NOT NULL,
	ForecastAgentDescription VARCHAR(250) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	DF_ForecastAgent_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	DF_ForecastAgent_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	DF_ForecastAgent_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	PK_ForecastAgent PRIMARY KEY CLUSTERED 
	(
	ForecastAgentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	FK_ForecastAgent_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgent].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgent', N'CONSTRAINT', N'FK_ForecastAgent_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ForecastAgent] ADD CONSTRAINT
	FK_ForecastAgent_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgent].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgent', N'CONSTRAINT', N'FK_ForecastAgent_SourceId'
GO
ALTER TABLE [DemandForecast].[ForecastAgent] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
