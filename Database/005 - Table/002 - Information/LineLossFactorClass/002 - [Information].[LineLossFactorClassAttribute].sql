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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[LineLossFactorClassAttribute]') AND type in (N'U'))
DROP TABLE [Information].[LineLossFactorClassAttribute]
GO
CREATE TABLE [Information].[LineLossFactorClassAttribute]
	(
	LineLossFactorClassAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LineLossFactorClassAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	PK_LineLossFactorClassAttribute PRIMARY KEY CLUSTERED 
	(
	LineLossFactorClassAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	DF_LineLossFactorClassAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	DF_LineLossFactorClassAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	DF_LineLossFactorClassAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	FK_LineLossFactorClassAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassAttribute', N'CONSTRAINT', N'FK_LineLossFactorClassAttribute_CreatedByUserId'
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] ADD CONSTRAINT
	FK_LineLossFactorClassAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassAttribute', N'CONSTRAINT', N'FK_LineLossFactorClassAttribute_SourceId'
GO
ALTER TABLE [Information].[LineLossFactorClassAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
