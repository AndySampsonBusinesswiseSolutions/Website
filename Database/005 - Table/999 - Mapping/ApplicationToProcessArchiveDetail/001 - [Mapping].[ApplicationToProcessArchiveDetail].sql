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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ApplicationToProcessArchiveDetail]') AND type in (N'U'))
DROP TABLE [Mapping].[ApplicationToProcessArchiveDetail]
GO
CREATE TABLE [Mapping].[ApplicationToProcessArchiveDetail]
	(
	ApplicationToProcessArchiveDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ApplicationId BIGINT NOT NULL,
	ProcessArchiveDetailId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	PK_ApplicationToProcessArchiveDetail PRIMARY KEY CLUSTERED 
	(
	ApplicationToProcessArchiveDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	DF_ApplicationToProcessArchiveDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	DF_ApplicationToProcessArchiveDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	DF_ApplicationToProcessArchiveDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	FK_ApplicationToProcessArchiveDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationToProcessArchiveDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationToProcessArchiveDetail', N'CONSTRAINT', N'FK_ApplicationToProcessArchiveDetail_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	FK_ApplicationToProcessArchiveDetail_ProcessArchiveDetailId FOREIGN KEY
	(
	ProcessArchiveDetailId
	) REFERENCES [System].[ProcessArchiveDetail]
	(
	ProcessArchiveDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationToProcessArchiveDetail].ProcessArchiveDetailId to [System].[ProcessArchiveDetail].ProcessArchiveDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationToProcessArchiveDetail', N'CONSTRAINT', N'FK_ApplicationToProcessArchiveDetail_ProcessArchiveDetailId'
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	FK_ApplicationToProcessArchiveDetail_ApplicationId FOREIGN KEY
	(
	ApplicationId
	) REFERENCES [System].[Application]
	(
	ApplicationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationToProcessArchiveDetail].ApplicationId to [System].[Application].ApplicationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationToProcessArchiveDetail', N'CONSTRAINT', N'FK_ApplicationToProcessArchiveDetail_ApplicationId'
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] ADD CONSTRAINT
	FK_ApplicationToProcessArchiveDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationToProcessArchiveDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationToProcessArchiveDetail', N'CONSTRAINT', N'FK_ApplicationToProcessArchiveDetail_SourceId'
GO
ALTER TABLE [Mapping].[ApplicationToProcessArchiveDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
