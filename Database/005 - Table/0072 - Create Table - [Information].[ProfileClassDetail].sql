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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[ProfileClassDetail]') AND type in (N'U'))
DROP TABLE [Information].[ProfileClassDetail]
GO
CREATE TABLE [Information].[ProfileClassDetail]
	(
	ProfileClassDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileClassId BIGINT NOT NULL,
	ProfileClassAttributeId BIGINT NOT NULL,
	ProfileClassDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	PK_ProfileClassDetail PRIMARY KEY CLUSTERED 
	(
	ProfileClassDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	DF_ProfileClassDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	DF_ProfileClassDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	DF_ProfileClassDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	FK_ProfileClassDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassDetail', N'CONSTRAINT', N'FK_ProfileClassDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	FK_ProfileClassDetail_ProfileClassId FOREIGN KEY
	(
	ProfileClassId
	) REFERENCES [Information].[ProfileClass]
	(
	ProfileClassId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassDetail].ProfileClassId to [Information].[ProfileClass].ProfileClassId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassDetail', N'CONSTRAINT', N'FK_ProfileClassDetail_ProfileClassId'
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	FK_ProfileClassDetail_ProfileClassAttributeId FOREIGN KEY
	(
	ProfileClassAttributeId
	) REFERENCES [Information].[ProfileClassAttribute]
	(
	ProfileClassAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassDetail].ProfileClassAttributeId to [Information].[ProfileClassAttribute].ProfileClassAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassDetail', N'CONSTRAINT', N'FK_ProfileClassDetail_ProfileClassAttributeId'
GO
ALTER TABLE [Information].[ProfileClassDetail] ADD CONSTRAINT
	FK_ProfileClassDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[ProfileClassDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'ProfileClassDetail', N'CONSTRAINT', N'FK_ProfileClassDetail_SourceId'
GO
ALTER TABLE [Information].[ProfileClassDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
