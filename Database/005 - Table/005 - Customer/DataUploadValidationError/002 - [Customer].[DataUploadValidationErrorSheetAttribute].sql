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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationErrorSheetAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationErrorSheetAttribute]
GO
CREATE TABLE [Customer].[DataUploadValidationErrorSheetAttribute]
	(
	DataUploadValidationErrorSheetAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorSheetAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	PK_DataUploadValidationErrorSheetAttribute PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorSheetAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheetAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheetAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorSheetAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheetAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheetAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheetAttribute', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheetAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] ADD CONSTRAINT
	FK_DataUploadValidationErrorSheetAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorSheetAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorSheetAttribute', N'CONSTRAINT', N'FK_DataUploadValidationErrorSheetAttribute_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorSheetAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
