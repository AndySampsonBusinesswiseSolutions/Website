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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[PasswordToUser]') AND type in (N'U'))
DROP TABLE [Mapping].[PasswordToUser]
GO
CREATE TABLE [Mapping].[PasswordToUser]
	(
	PasswordToUserId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	UserId BIGINT NOT NULL,
	PasswordId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	PK_PasswordToUser PRIMARY KEY CLUSTERED 
	(
	PasswordToUserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	DF_PasswordToUser_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	DF_PasswordToUser_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	DF_PasswordToUser_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	FK_PasswordToUser_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PasswordToUser].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PasswordToUser', N'CONSTRAINT', N'FK_PasswordToUser_CreatedByUserId'
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	FK_PasswordToUser_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PasswordToUser].UserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PasswordToUser', N'CONSTRAINT', N'FK_PasswordToUser_UserId'
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	FK_PasswordToUser_PasswordId FOREIGN KEY
	(
	PasswordId
	) REFERENCES [Administration.User].[Password]
	(
	PasswordId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PasswordToUser].PasswordId to [Administration.User].[Password].PasswordId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PasswordToUser', N'CONSTRAINT', N'FK_PasswordToUser_PasswordId'
GO
ALTER TABLE [Mapping].[PasswordToUser] ADD CONSTRAINT
	FK_PasswordToUser_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PasswordToUser].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PasswordToUser', N'CONSTRAINT', N'FK_PasswordToUser_SourceId'
GO
ALTER TABLE [Mapping].[PasswordToUser] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
