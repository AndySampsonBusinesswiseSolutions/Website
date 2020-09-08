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
CREATE TABLE [DemandForecast].[ProfileAgentDetail]
	(
	ProfileAgentDetailId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileAgentId BIGINT NOT NULL,
	ProfileAgentAttributeId BIGINT NOT NULL,
	ProfileAgentDetailDescription VARCHAR(255) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	DF_ProfileAgentDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	DF_ProfileAgentDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	DF_ProfileAgentDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	PK_ProfileAgentDetail PRIMARY KEY CLUSTERED 
	(
	ProfileAgentDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	FK_ProfileAgentDetail_ProfileAgentId FOREIGN KEY
	(
	ProfileAgentId
	) REFERENCES [DemandForecast].[ProfileAgent]
	(
	ProfileAgentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentDetail].SourceId to [DemandForecast].[ProfileAgent].ProfileAgentId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentDetail', N'CONSTRAINT', N'FK_ProfileAgentDetail_ProfileAgentId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	FK_ProfileAgentDetail_ProfileAgentAttributeId FOREIGN KEY
	(
	ProfileAgentAttributeId
	) REFERENCES [DemandForecast].[ProfileAgentAttribute]
	(
	ProfileAgentAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentDetail].ProfileAgentAttributeId to [DemandForecast].[ProfileAgentAttribute].ProfileAgentAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentDetail', N'CONSTRAINT', N'FK_ProfileAgentDetail_ProfileAgentAttributeId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	FK_ProfileAgentDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentDetail', N'CONSTRAINT', N'FK_ProfileAgentDetail_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] ADD CONSTRAINT
	FK_ProfileAgentDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentDetail', N'CONSTRAINT', N'FK_ProfileAgentDetail_SourceId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
