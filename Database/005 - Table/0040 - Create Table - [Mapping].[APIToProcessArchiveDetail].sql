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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[APIToProcessArchiveDetail]') AND type in (N'U'))
DROP TABLE [Mapping].[APIToProcessArchiveDetail]
GO
CREATE TABLE [Mapping].[APIToProcessArchiveDetail]
	(
	APIToProcessArchiveDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	APIId BIGINT NOT NULL,
	ProcessArchiveDetailId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	PK_APIToProcessArchiveDetail PRIMARY KEY CLUSTERED 
	(
	APIToProcessArchiveDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	DF_APIToProcessArchiveDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	DF_APIToProcessArchiveDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	DF_APIToProcessArchiveDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	FK_APIToProcessArchiveDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcessArchiveDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcessArchiveDetail', N'CONSTRAINT', N'FK_APIToProcessArchiveDetail_CreatedByUserId'
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	FK_APIToProcessArchiveDetail_ProcessArchiveDetailId FOREIGN KEY
	(
	ProcessArchiveDetailId
	) REFERENCES [System].[ProcessArchiveDetail]
	(
	ProcessArchiveDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcessArchiveDetail].ProcessArchiveDetailId to [System].[ProcessArchiveDetail].ProcessArchiveDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcessArchiveDetail', N'CONSTRAINT', N'FK_APIToProcessArchiveDetail_ProcessArchiveDetailId'
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	FK_APIToProcessArchiveDetail_APIId FOREIGN KEY
	(
	APIId
	) REFERENCES [System].[API]
	(
	APIId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcessArchiveDetail].APIId to [System].[API].APIId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcessArchiveDetail', N'CONSTRAINT', N'FK_APIToProcessArchiveDetail_APIId'
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] ADD CONSTRAINT
	FK_APIToProcessArchiveDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcessArchiveDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcessArchiveDetail', N'CONSTRAINT', N'FK_APIToProcessArchiveDetail_SourceId'
GO
ALTER TABLE [Mapping].[APIToProcessArchiveDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
