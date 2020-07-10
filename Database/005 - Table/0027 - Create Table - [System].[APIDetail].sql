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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[APIDetail]') AND type in (N'U'))
DROP TABLE [System].[APIDetail]
GO
CREATE TABLE [System].[APIDetail]
	(
	APIDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	APIId BIGINT NOT NULL,
	APIAttributeId BIGINT NOT NULL,
	APIDetailDescription VARCHAR(255) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	PK_APIDetail PRIMARY KEY CLUSTERED 
	(
	APIDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	DF_APIDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	DF_APIDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	DF_APIDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	FK_APIDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIDetail', N'CONSTRAINT', N'FK_APIDetail_CreatedByUserId'
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	FK_APIDetail_APIId FOREIGN KEY
	(
	APIId
	) REFERENCES [System].[API]
	(
	APIId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIDetail].APIId to [System].[API].APIId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIDetail', N'CONSTRAINT', N'FK_APIDetail_APIId'
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	FK_APIDetail_APIAttributeId FOREIGN KEY
	(
	APIAttributeId
	) REFERENCES [System].[APIAttribute]
	(
	APIAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIDetail].APIAttributeId to [System].[APIAttribute].APIAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIDetail', N'CONSTRAINT', N'FK_APIDetail_APIAttributeId'
GO
ALTER TABLE [System].[APIDetail] ADD CONSTRAINT
	FK_APIDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIDetail', N'CONSTRAINT', N'FK_APIDetail_SourceId'
GO
ALTER TABLE [System].[APIDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
