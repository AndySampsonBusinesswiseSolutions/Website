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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[Source]') AND type in (N'U'))
DROP TABLE [Information].[Source]
GO
CREATE TABLE [Information].[Source]
	(
	SourceId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceTypeId bigint NOT NULL,
	SourceTypeEntityId bigint NOT NULL
	)  ON Information
GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	PK_Source PRIMARY KEY CLUSTERED 
	(
	SourceId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Information

GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	DF_Source_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	DF_Source_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	DF_Source_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	FK_Source_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[Source].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'Source', N'CONSTRAINT', N'FK_Source_CreatedByUserId'
GO
ALTER TABLE [Information].[Source] ADD CONSTRAINT
	FK_Source_SourceTypeId FOREIGN KEY
	(
	SourceTypeId
	) REFERENCES [Information].[SourceType]
	(
	SourceTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[Source].SourceTypeId to [Information].[SourceType].SourceTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'Source', N'CONSTRAINT', N'FK_Source_SourceTypeId'
GO
ALTER TABLE [Information].[Source] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
