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
CREATE TABLE [DemandForecast].[ForecastAgentDetail]
	(
	ForecastAgentDetailId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ForecastAgentId BIGINT NOT NULL,
	ForecastAgentAttributeId BIGINT NOT NULL,
	ForecastAgentDetailDescription VARCHAR(255) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	DF_ForecastAgentDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	DF_ForecastAgentDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	DF_ForecastAgentDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	PK_ForecastAgentDetail PRIMARY KEY CLUSTERED 
	(
	ForecastAgentDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	FK_ForecastAgentDetail_ForecastAgentId FOREIGN KEY
	(
	ForecastAgentId
	) REFERENCES [DemandForecast].[ForecastAgent]
	(
	ForecastAgentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgentDetail].SourceId to [DemandForecast].[ForecastAgent].ForecastAgentId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgentDetail', N'CONSTRAINT', N'FK_ForecastAgentDetail_ForecastAgentId'
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	FK_ForecastAgentDetail_ForecastAgentAttributeId FOREIGN KEY
	(
	ForecastAgentAttributeId
	) REFERENCES [DemandForecast].[ForecastAgentAttribute]
	(
	ForecastAgentAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgentDetail].ForecastAgentAttributeId to [DemandForecast].[ForecastAgentAttribute].ForecastAgentAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgentDetail', N'CONSTRAINT', N'FK_ForecastAgentDetail_ForecastAgentAttributeId'
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	FK_ForecastAgentDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgentDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgentDetail', N'CONSTRAINT', N'FK_ForecastAgentDetail_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] ADD CONSTRAINT
	FK_ForecastAgentDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ForecastAgentDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ForecastAgentDetail', N'CONSTRAINT', N'FK_ForecastAgentDetail_SourceId'
GO
ALTER TABLE [DemandForecast].[ForecastAgentDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
