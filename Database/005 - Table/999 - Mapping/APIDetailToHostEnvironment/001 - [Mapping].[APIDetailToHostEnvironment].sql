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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[APIDetailToHostEnvironment]') AND type in (N'U'))
DROP TABLE [Mapping].[APIDetailToHostEnvironment]
GO
CREATE TABLE [Mapping].[APIDetailToHostEnvironment]
	(
	APIDetailToHostEnvironmentId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	APIDetailId BIGINT NOT NULL,
	HostEnvironmentId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	PK_APIDetailToHostEnvironment PRIMARY KEY CLUSTERED 
	(
	APIDetailToHostEnvironmentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	DF_APIDetailToHostEnvironment_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	DF_APIDetailToHostEnvironment_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	DF_APIDetailToHostEnvironment_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	FK_APIDetailToHostEnvironment_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIDetailToHostEnvironment].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIDetailToHostEnvironment', N'CONSTRAINT', N'FK_APIDetailToHostEnvironment_CreatedByUserId'
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	FK_APIDetailToHostEnvironment_HostEnvironmentId FOREIGN KEY
	(
	HostEnvironmentId
	) REFERENCES [System].[HostEnvironment]
	(
	HostEnvironmentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIDetailToHostEnvironment].HostEnvironmentId to [System].[HostEnvironment].HostEnvironmentId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIDetailToHostEnvironment', N'CONSTRAINT', N'FK_APIDetailToHostEnvironment_HostEnvironmentId'
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	FK_APIDetailToHostEnvironment_APIDetailId FOREIGN KEY
	(
	APIDetailId
	) REFERENCES [System].[APIDetail]
	(
	APIDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIDetailToHostEnvironment].APIDetailId to [System].[APIDetail].APIDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIDetailToHostEnvironment', N'CONSTRAINT', N'FK_APIDetailToHostEnvironment_APIDetailId'
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] ADD CONSTRAINT
	FK_APIDetailToHostEnvironment_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIDetailToHostEnvironment].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIDetailToHostEnvironment', N'CONSTRAINT', N'FK_APIDetailToHostEnvironment_SourceId'
GO
ALTER TABLE [Mapping].[APIDetailToHostEnvironment] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
