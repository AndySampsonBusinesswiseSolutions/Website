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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[SourceDetail]') AND type in (N'U'))
DROP TABLE [Information].[SourceDetail]
GO
CREATE TABLE [Information].[SourceDetail]
	(
	SourceDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	SourceAttributeId BIGINT NOT NULL,
	SourceDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	PK_SourceDetail PRIMARY KEY CLUSTERED 
	(
	SourceDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	DF_SourceDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	DF_SourceDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	DF_SourceDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	FK_SourceDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceDetail', N'CONSTRAINT', N'FK_SourceDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	FK_SourceDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceDetail', N'CONSTRAINT', N'FK_SourceDetail_SourceId'
GO
ALTER TABLE [Information].[SourceDetail] ADD CONSTRAINT
	FK_SourceDetail_SourceAttributeId FOREIGN KEY
	(
	SourceAttributeId
	) REFERENCES [Information].[SourceAttribute]
	(
	SourceAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceDetail].SourceAttributeId to [Information].[SourceAttribute].SourceAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceDetail', N'CONSTRAINT', N'FK_SourceDetail_SourceAttributeId'
GO
ALTER TABLE [Information].[SourceDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
