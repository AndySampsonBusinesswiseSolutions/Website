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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[LineLossFactorClassDetail]') AND type in (N'U'))
DROP TABLE [Information].[LineLossFactorClassDetail]
GO
CREATE TABLE [Information].[LineLossFactorClassDetail]
	(
	LineLossFactorClassDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LineLossFactorClassId BIGINT NOT NULL,
	LineLossFactorClassAttributeId BIGINT NOT NULL,
	LineLossFactorClassDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	PK_LineLossFactorClassDetail PRIMARY KEY CLUSTERED 
	(
	LineLossFactorClassDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	DF_LineLossFactorClassDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	DF_LineLossFactorClassDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	DF_LineLossFactorClassDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	FK_LineLossFactorClassDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassDetail', N'CONSTRAINT', N'FK_LineLossFactorClassDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	FK_LineLossFactorClassDetail_LineLossFactorClassId FOREIGN KEY
	(
	LineLossFactorClassId
	) REFERENCES [Information].[LineLossFactorClass]
	(
	LineLossFactorClassId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassDetail].LineLossFactorClassId to [Information].[LineLossFactorClass].LineLossFactorClassId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassDetail', N'CONSTRAINT', N'FK_LineLossFactorClassDetail_LineLossFactorClassId'
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	FK_LineLossFactorClassDetail_LineLossFactorClassAttributeId FOREIGN KEY
	(
	LineLossFactorClassAttributeId
	) REFERENCES [Information].[LineLossFactorClassAttribute]
	(
	LineLossFactorClassAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassDetail].LineLossFactorClassAttributeId to [Information].[LineLossFactorClassAttribute].LineLossFactorClassAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassDetail', N'CONSTRAINT', N'FK_LineLossFactorClassDetail_LineLossFactorClassAttributeId'
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] ADD CONSTRAINT
	FK_LineLossFactorClassDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[LineLossFactorClassDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'LineLossFactorClassDetail', N'CONSTRAINT', N'FK_LineLossFactorClassDetail_SourceId'
GO
ALTER TABLE [Information].[LineLossFactorClassDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
