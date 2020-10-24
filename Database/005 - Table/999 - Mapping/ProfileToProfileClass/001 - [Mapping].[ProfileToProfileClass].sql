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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ProfileToProfileClass]') AND type in (N'U'))
DROP TABLE [Mapping].[ProfileToProfileClass]
GO
CREATE TABLE [Mapping].[ProfileToProfileClass]
	(
	ProfileToProfileClassId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileId BIGINT NOT NULL,
	ProfileClassId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	PK_ProfileToProfileClass PRIMARY KEY CLUSTERED 
	(
	ProfileToProfileClassId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	DF_ProfileToProfileClass_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	DF_ProfileToProfileClass_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	DF_ProfileToProfileClass_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	FK_ProfileToProfileClass_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileClass].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileClass', N'CONSTRAINT', N'FK_ProfileToProfileClass_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	FK_ProfileToProfileClass_ProfileClassId FOREIGN KEY
	(
	ProfileClassId
	) REFERENCES [Information].[ProfileClass]
	(
	ProfileClassId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileClass].ProfileClassId to [Information].[ProfileClass].ProfileClassId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileClass', N'CONSTRAINT', N'FK_ProfileToProfileClass_ProfileClassId'
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	FK_ProfileToProfileClass_ProfileId FOREIGN KEY
	(
	ProfileId
	) REFERENCES [DemandForecast].[Profile]
	(
	ProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileClass].ProfileId to [DemandForecast].[Profile].ProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileClass', N'CONSTRAINT', N'FK_ProfileToProfileClass_ProfileId'
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] ADD CONSTRAINT
	FK_ProfileToProfileClass_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProfileToProfileClass].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProfileToProfileClass', N'CONSTRAINT', N'FK_ProfileToProfileClass_SourceId'
GO
ALTER TABLE [Mapping].[ProfileToProfileClass] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
