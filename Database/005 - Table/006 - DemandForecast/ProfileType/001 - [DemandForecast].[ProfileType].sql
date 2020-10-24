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
CREATE TABLE [DemandForecast].[ProfileType]
	(
	ProfileTypeId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileTypeDescription VARCHAR(255) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	DF_ProfileType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	DF_ProfileType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	DF_ProfileType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	PK_ProfileType PRIMARY KEY CLUSTERED 
	(
	ProfileTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	FK_ProfileType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileType', N'CONSTRAINT', N'FK_ProfileType_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ProfileType] ADD CONSTRAINT
	FK_ProfileType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileType', N'CONSTRAINT', N'FK_ProfileType_SourceId'
GO
ALTER TABLE [DemandForecast].[ProfileType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
