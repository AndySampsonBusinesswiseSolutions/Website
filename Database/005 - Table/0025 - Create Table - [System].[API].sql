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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[API]') AND type in (N'U'))
DROP TABLE [System].[API]
GO
CREATE TABLE [System].[API]
	(
	APIId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	APIGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [System]
GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	PK_API PRIMARY KEY CLUSTERED 
	(
	APIId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	DF_API_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	DF_API_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	DF_API_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	FK_API_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[API].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'API', N'CONSTRAINT', N'FK_API_CreatedByUserId'
GO
ALTER TABLE [System].[API] ADD CONSTRAINT
	FK_API_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[API].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'API', N'CONSTRAINT', N'FK_API_SourceId'
GO
ALTER TABLE [System].[API] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
