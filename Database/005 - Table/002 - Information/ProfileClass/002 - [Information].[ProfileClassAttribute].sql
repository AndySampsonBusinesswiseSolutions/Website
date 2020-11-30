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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[ProfileClassAttribute]') AND type in (N'U'))
DROP TABLE [Information].[ProfileClassAttribute]
GO
CREATE TABLE [Information].[ProfileClassAttribute]
	(
	ProfileClassAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileClassAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	PK_ProfileClassAttribute PRIMARY KEY CLUSTERED 
	(
	ProfileClassAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	DF_ProfileClassAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	DF_ProfileClassAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	DF_ProfileClassAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	FK_ProfileClassAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassAttribute', N'CONSTRAINT', N'FK_ProfileClassAttribute_CreatedByUserId'
GO
ALTER TABLE [Information].[ProfileClassAttribute] ADD CONSTRAINT
	FK_ProfileClassAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassAttribute', N'CONSTRAINT', N'FK_ProfileClassAttribute_SourceId'
GO
ALTER TABLE [Information].[ProfileClassAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
