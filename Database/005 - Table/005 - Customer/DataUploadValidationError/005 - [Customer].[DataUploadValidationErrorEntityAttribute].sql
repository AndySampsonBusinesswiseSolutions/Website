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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationErrorEntityAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationErrorEntityAttribute]
GO
CREATE TABLE [Customer].[DataUploadValidationErrorEntityAttribute]
	(
	DataUploadValidationErrorEntityAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorEntityAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	PK_DataUploadValidationErrorEntityAttribute PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorEntityAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntityAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntityAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntityAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntityAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntityAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntityAttribute', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntityAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntityAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntityAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntityAttribute', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntityAttribute_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntityAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
