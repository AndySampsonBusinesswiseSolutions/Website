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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationErrorEntity]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationErrorEntity]
GO
CREATE TABLE [Customer].[DataUploadValidationErrorEntity]
	(
	DataUploadValidationErrorEntityId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorRowId BIGINT NOT NULL,
	DataUploadValidationErrorEntityAttributeId BIGINT NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	PK_DataUploadValidationErrorEntity PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorEntityId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntity_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntity_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	DF_DataUploadValidationErrorEntity_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntity_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntity].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntity', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntity_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntity_DataUploadValidationErrorRowId FOREIGN KEY
	(
	DataUploadValidationErrorRowId
	) REFERENCES [Customer].[DataUploadValidationErrorRow]
	(
	DataUploadValidationErrorRowId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntity].DataUploadValidationErrorRowId to [Customer].[DataUploadValidationErrorRow].DataUploadValidationErrorRowId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntity', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntity_DataUploadValidationErrorRowId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntity_DataUploadValidationErrorEntityAttributeId FOREIGN KEY
	(
	DataUploadValidationErrorEntityAttributeId
	) REFERENCES [Customer].[DataUploadValidationErrorEntityAttribute]
	(
	DataUploadValidationErrorEntityAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntity].DataUploadValidationErrorEntityAttributeId to [Customer].[DataUploadValidationErrorEntityAttribute].DataUploadValidationErrorEntityAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntity', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntity_DataUploadValidationErrorEntityAttributeId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] ADD CONSTRAINT
	FK_DataUploadValidationErrorEntity_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorEntity].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorEntity', N'CONSTRAINT', N'FK_DataUploadValidationErrorEntity_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorEntity] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
