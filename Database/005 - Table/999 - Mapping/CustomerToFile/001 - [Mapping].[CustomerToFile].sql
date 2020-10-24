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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CustomerToFile]') AND type in (N'U'))
DROP TABLE [Mapping].[CustomerToFile]
GO
CREATE TABLE [Mapping].[CustomerToFile]
	(
	CustomerToFileId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CustomerId BIGINT NOT NULL,
	FileId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	PK_CustomerToFile PRIMARY KEY CLUSTERED 
	(
	CustomerToFileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	DF_CustomerToFile_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	DF_CustomerToFile_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	DF_CustomerToFile_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	FK_CustomerToFile_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToFile].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToFile', N'CONSTRAINT', N'FK_CustomerToFile_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	FK_CustomerToFile_FileId FOREIGN KEY
	(
	FileId
	) REFERENCES [Information].[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToFile].FileId to [Information].[File].FileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToFile', N'CONSTRAINT', N'FK_CustomerToFile_FileId'
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	FK_CustomerToFile_CustomerId FOREIGN KEY
	(
	CustomerId
	) REFERENCES [Customer].[Customer]
	(
	CustomerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToFile].CustomerId to [Customer].[Customer].CustomerId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToFile', N'CONSTRAINT', N'FK_CustomerToFile_CustomerId'
GO
ALTER TABLE [Mapping].[CustomerToFile] ADD CONSTRAINT
	FK_CustomerToFile_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToFile].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToFile', N'CONSTRAINT', N'FK_CustomerToFile_SourceId'
GO
ALTER TABLE [Mapping].[CustomerToFile] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
