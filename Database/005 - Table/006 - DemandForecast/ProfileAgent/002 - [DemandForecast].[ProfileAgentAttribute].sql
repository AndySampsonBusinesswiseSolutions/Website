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
CREATE TABLE [DemandForecast].[ProfileAgentAttribute]
	(
	ProfileAgentAttributeId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileAgentAttributeDescription VARCHAR(255) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	DF_ProfileAgentAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	DF_ProfileAgentAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	DF_ProfileAgentAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	PK_ProfileAgentAttribute PRIMARY KEY CLUSTERED 
	(
	ProfileAgentAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	FK_ProfileAgentAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentAttribute', N'CONSTRAINT', N'FK_ProfileAgentAttribute_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] ADD CONSTRAINT
	FK_ProfileAgentAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileAgentAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileAgentAttribute', N'CONSTRAINT', N'FK_ProfileAgentAttribute_SourceId'
GO
ALTER TABLE [DemandForecast].[ProfileAgentAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
