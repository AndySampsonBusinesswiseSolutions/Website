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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[DataUploadValidationErrorDetail]') AND type in (N'U'))
DROP TABLE [Customer].[DataUploadValidationErrorDetail]
GO
CREATE TABLE [Customer].[DataUploadValidationErrorDetail]
	(
	DataUploadValidationErrorDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DataUploadValidationErrorId BIGINT NOT NULL,
	DataUploadValidationErrorAttributeId BIGINT NOT NULL,
	DataUploadValidationErrorDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	PK_DataUploadValidationErrorDetail PRIMARY KEY CLUSTERED 
	(
	DataUploadValidationErrorDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	DF_DataUploadValidationErrorDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	DF_DataUploadValidationErrorDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	DF_DataUploadValidationErrorDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	FK_DataUploadValidationErrorDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorDetail', N'CONSTRAINT', N'FK_DataUploadValidationErrorDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	FK_DataUploadValidationErrorDetail_DataUploadValidationErrorId FOREIGN KEY
	(
	DataUploadValidationErrorId
	) REFERENCES [Customer].[DataUploadValidationError]
	(
	DataUploadValidationErrorId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorDetail].DataUploadValidationErrorId to [Customer].[DataUploadValidationError].DataUploadValidationErrorId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorDetail', N'CONSTRAINT', N'FK_DataUploadValidationErrorDetail_DataUploadValidationErrorId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	FK_DataUploadValidationErrorDetail_DataUploadValidationErrorAttributeId FOREIGN KEY
	(
	DataUploadValidationErrorAttributeId
	) REFERENCES [Customer].[DataUploadValidationErrorAttribute]
	(
	DataUploadValidationErrorAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorDetail].DataUploadValidationErrorAttributeId to [Customer].[DataUploadValidationErrorAttribute].DataUploadValidationErrorAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorDetail', N'CONSTRAINT', N'FK_DataUploadValidationErrorDetail_DataUploadValidationErrorAttributeId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] ADD CONSTRAINT
	FK_DataUploadValidationErrorDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[DataUploadValidationErrorDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'DataUploadValidationErrorDetail', N'CONSTRAINT', N'FK_DataUploadValidationErrorDetail_SourceId'
GO
ALTER TABLE [Customer].[DataUploadValidationErrorDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
