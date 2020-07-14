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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[FileContent]') AND type in (N'U'))
DROP TABLE [Information].[FileContent]
GO
CREATE TABLE [Information].[FileContent]
	(
	FileContentId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FileId BIGINT NOT NULL,
	FileContent NVARCHAR(MAX) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	PK_FileContent PRIMARY KEY CLUSTERED 
	(
	FileContentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	DF_FileContent_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	DF_FileContent_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	DF_FileContent_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	FK_FileContent_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileContent].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileContent', N'CONSTRAINT', N'FK_FileContent_CreatedByUserId'
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	FK_FileContent_FileId FOREIGN KEY
	(
	FileId
	) REFERENCES [Information].[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileContent].FileId to [Information].[File].FileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileContent', N'CONSTRAINT', N'FK_FileContent_FileId'
GO
ALTER TABLE [Information].[FileContent] ADD CONSTRAINT
	FK_FileContent_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileContent].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileContent', N'CONSTRAINT', N'FK_FileContent_SourceId'
GO
ALTER TABLE [Information].[FileContent] ADD INDEX IX_FileContent_ClusteredColumnstore CLUSTERED COLUMNSTORE
GO
ALTER TABLE [Information].[FileContent] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
