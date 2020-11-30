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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ApplicationDetail]') AND type in (N'U'))
DROP TABLE [System].[ApplicationDetail]
GO
CREATE TABLE [System].[ApplicationDetail]
	(
	ApplicationDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ApplicationId BIGINT NOT NULL,
	ApplicationAttributeId BIGINT NOT NULL,
	ApplicationDetailDescription VARCHAR(255) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	PK_ApplicationDetail PRIMARY KEY CLUSTERED 
	(
	ApplicationDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	DF_ApplicationDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	DF_ApplicationDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	DF_ApplicationDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	FK_ApplicationDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationDetail', N'CONSTRAINT', N'FK_ApplicationDetail_CreatedByUserId'
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	FK_ApplicationDetail_ApplicationId FOREIGN KEY
	(
	ApplicationId
	) REFERENCES [System].[Application]
	(
	ApplicationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationDetail].ApplicationId to [System].[Application].ApplicationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationDetail', N'CONSTRAINT', N'FK_ApplicationDetail_ApplicationId'
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	FK_ApplicationDetail_ApplicationAttributeId FOREIGN KEY
	(
	ApplicationAttributeId
	) REFERENCES [System].[ApplicationAttribute]
	(
	ApplicationAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationDetail].ApplicationAttributeId to [System].[ApplicationAttribute].ApplicationAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationDetail', N'CONSTRAINT', N'FK_ApplicationDetail_ApplicationAttributeId'
GO
ALTER TABLE [System].[ApplicationDetail] ADD CONSTRAINT
	FK_ApplicationDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationDetail', N'CONSTRAINT', N'FK_ApplicationDetail_SourceId'
GO
ALTER TABLE [System].[ApplicationDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
