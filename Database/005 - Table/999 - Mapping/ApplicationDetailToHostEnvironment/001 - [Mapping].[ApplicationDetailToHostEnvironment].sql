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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ApplicationDetailToHostEnvironment]') AND type in (N'U'))
DROP TABLE [Mapping].[ApplicationDetailToHostEnvironment]
GO
CREATE TABLE [Mapping].[ApplicationDetailToHostEnvironment]
	(
	ApplicationDetailToHostEnvironmentId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ApplicationDetailId BIGINT NOT NULL,
	HostEnvironmentId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	PK_ApplicationDetailToHostEnvironment PRIMARY KEY CLUSTERED 
	(
	ApplicationDetailToHostEnvironmentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	DF_ApplicationDetailToHostEnvironment_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	DF_ApplicationDetailToHostEnvironment_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	DF_ApplicationDetailToHostEnvironment_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	FK_ApplicationDetailToHostEnvironment_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationDetailToHostEnvironment].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationDetailToHostEnvironment', N'CONSTRAINT', N'FK_ApplicationDetailToHostEnvironment_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	FK_ApplicationDetailToHostEnvironment_HostEnvironmentId FOREIGN KEY
	(
	HostEnvironmentId
	) REFERENCES [System].[HostEnvironment]
	(
	HostEnvironmentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationDetailToHostEnvironment].HostEnvironmentId to [System].[HostEnvironment].HostEnvironmentId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationDetailToHostEnvironment', N'CONSTRAINT', N'FK_ApplicationDetailToHostEnvironment_HostEnvironmentId'
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	FK_ApplicationDetailToHostEnvironment_ApplicationDetailId FOREIGN KEY
	(
	ApplicationDetailId
	) REFERENCES [System].[ApplicationDetail]
	(
	ApplicationDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationDetailToHostEnvironment].ApplicationDetailId to [System].[ApplicationDetail].ApplicationDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationDetailToHostEnvironment', N'CONSTRAINT', N'FK_ApplicationDetailToHostEnvironment_ApplicationDetailId'
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] ADD CONSTRAINT
	FK_ApplicationDetailToHostEnvironment_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ApplicationDetailToHostEnvironment].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ApplicationDetailToHostEnvironment', N'CONSTRAINT', N'FK_ApplicationDetailToHostEnvironment_SourceId'
GO
ALTER TABLE [Mapping].[ApplicationDetailToHostEnvironment] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
