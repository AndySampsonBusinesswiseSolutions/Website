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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[LoginToUser]') AND type in (N'U'))
DROP TABLE [Mapping].[LoginToUser]
GO
CREATE TABLE [Mapping].[LoginToUser]
	(
	LoginToUserId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	UserId BIGINT NOT NULL,
	LoginId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	PK_LoginToUser PRIMARY KEY CLUSTERED 
	(
	LoginToUserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	DF_LoginToUser_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	DF_LoginToUser_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	DF_LoginToUser_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	FK_LoginToUser_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToUser].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToUser', N'CONSTRAINT', N'FK_LoginToUser_CreatedByUserId'
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	FK_LoginToUser_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToUser].UserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToUser', N'CONSTRAINT', N'FK_LoginToUser_UserId'
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	FK_LoginToUser_LoginId FOREIGN KEY
	(
	LoginId
	) REFERENCES [Administration.User].[Login]
	(
	LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToUser].LoginId to [Administration.User].[Login].LoginId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToUser', N'CONSTRAINT', N'FK_LoginToUser_LoginId'
GO
ALTER TABLE [Mapping].[LoginToUser] ADD CONSTRAINT
	FK_Login_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToUser].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToUser', N'CONSTRAINT', N'FK_Login_SourceId'
GO
ALTER TABLE [Mapping].[LoginToUser] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
