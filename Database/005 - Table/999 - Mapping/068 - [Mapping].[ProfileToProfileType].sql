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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ProfileToProfileType]') AND type in (N'U'))
DROP TABLE [Mapping].[ProfileToProfileType]
GO
CREATE TABLE [Mapping].[ProfileToProfileType]
	(
	ProfileToProfileTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileId BIGINT NOT NULL,
	ProfileTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	PK_ProfileToProfileType PRIMARY KEY CLUSTERED 
	(
	ProfileToProfileTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	DF_ProfileToProfileType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	DF_ProfileToProfileType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	DF_ProfileToProfileType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	FK_ProfileToProfileType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileType', N'CONSTRAINT', N'FK_ProfileToProfileType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	FK_ProfileToProfileType_ProfileTypeId FOREIGN KEY
	(
	ProfileTypeId
	) REFERENCES [DemandForecast].[ProfileType]
	(
	ProfileTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileType].ProfileTypeId to [DemandForecast].[ProfileType].ProfileTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileType', N'CONSTRAINT', N'FK_ProfileToProfileType_ProfileTypeId'
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	FK_ProfileToProfileType_ProfileId FOREIGN KEY
	(
	ProfileId
	) REFERENCES [DemandForecast].[Profile]
	(
	ProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileType].ProfileId to [DemandForecast].[Profile].ProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileType', N'CONSTRAINT', N'FK_ProfileToProfileType_ProfileId'
GO
ALTER TABLE [Mapping].[ProfileToProfileType] ADD CONSTRAINT
	FK_ProfileToProfileType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileType', N'CONSTRAINT', N'FK_ProfileToProfileType_SourceId'
GO
ALTER TABLE [Mapping].[ProfileToProfileType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
