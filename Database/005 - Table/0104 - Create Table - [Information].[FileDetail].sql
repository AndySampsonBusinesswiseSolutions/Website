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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[FileDetail]') AND type in (N'U'))
DROP TABLE [Information].[FileDetail]
GO
CREATE TABLE [Information].[FileDetail]
	(
	FileDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FileId BIGINT NOT NULL,
	FileAttributeId BIGINT NOT NULL,
	FileDetailDescription VARCHAR(200) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	PK_FileDetail PRIMARY KEY CLUSTERED 
	(
	FileDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	DF_FileDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	DF_FileDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	DF_FileDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	FK_FileDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileDetail', N'CONSTRAINT', N'FK_FileDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	FK_FileDetail_FileId FOREIGN KEY
	(
	FileId
	) REFERENCES [Information].[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileDetail].FileId to [Information].[File].FileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileDetail', N'CONSTRAINT', N'FK_FileDetail_FileId'
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	FK_FileDetail_FileAttributeId FOREIGN KEY
	(
	FileAttributeId
	) REFERENCES [Information].[FileAttribute]
	(
	FileAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileDetail].FileAttributeId to [Information].[FileAttribute].FileAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileDetail', N'CONSTRAINT', N'FK_FileDetail_FileAttributeId'
GO
ALTER TABLE [Information].[FileDetail] ADD CONSTRAINT
	FK_FileDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FileDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FileDetail', N'CONSTRAINT', N'FK_FileDetail_SourceId'
GO
ALTER TABLE [Information].[FileDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
