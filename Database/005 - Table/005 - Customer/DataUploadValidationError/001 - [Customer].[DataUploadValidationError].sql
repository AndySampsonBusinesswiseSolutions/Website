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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationError]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationError]
GO
CREATE TABLE [Customer].[DataUploadValidationError]
	(
	DataUploadValidationErrorId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorGUID UNIQUEIDENTIFIER NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	PK_DataUploadValidationError PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	DF_DataUploadValidationError_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	DF_DataUploadValidationError_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	DF_DataUploadValidationError_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	FK_DataUploadValidationError_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationError].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationError', N'CONSTRAINT', N'FK_DataUploadValidationError_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationError] ADD CONSTRAINT
	FK_DataUploadValidationError_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationError].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationError', N'CONSTRAINT', N'FK_DataUploadValidationError_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationError] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
