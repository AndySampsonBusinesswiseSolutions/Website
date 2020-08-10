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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationErrorSheet]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationErrorSheet]
GO
CREATE TABLE [Customer].[DataUploadValidationErrorSheet]
	(
	DataUploadValidationErrorSheetId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorId BIGINT NOT NULL,
	DataUploadValidationErrorSheetAttributeId BIGINT NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	PK_DataUploadValidationErrorSheet PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorSheetId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheet_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheet_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheet_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheet_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheet].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheet', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheet_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheet_DataUploadValidationErrorId FOREIGN KEY
	(
	DataUploadValidationErrorId
	) REFERENCES [Customer].[DataUploadValidationError]
	(
	DataUploadValidationErrorId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheet].DataUploadValidationErrorId to [Customer].[DataUploadValidationError].DataUploadValidationErrorId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheet', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheet_DataUploadValidationErrorId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheet_DataUploadValidationErrorSheetAttributeId FOREIGN KEY
	(
	DataUploadValidationErrorSheetAttributeId
	) REFERENCES [Customer].[DataUploadValidationErrorSheetAttribute]
	(
	DataUploadValidationErrorSheetAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheet].DataUploadValidationErrorSheetAttributeId to [Customer].[DataUploadValidationErrorSheetAttribute].DataUploadValidationErrorSheetAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheet', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheet_DataUploadValidationErrorSheetAttributeId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheet_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheet].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheet', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheet_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheet] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
