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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[RoleToUser]') AND type in (N'U'))
DROP TABLE [Mapping].[RoleToUser]
GO
CREATE TABLE [Mapping].[RoleToUser]
	(
	RoleToUserId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	RoleId bigint NOT NULL,
	UserId bigint NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	PK_RoleToUser PRIMARY KEY CLUSTERED 
	(
	RoleToUserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	DF_RoleToUser_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	DF_RoleToUser_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	DF_RoleToUser_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	FK_RoleToUser_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[RoleToUser].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'RoleToUser', N'CONSTRAINT', N'FK_RoleToUser_CreatedByUserId'
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	FK_RoleToUser_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[RoleToUser].UserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'RoleToUser', N'CONSTRAINT', N'FK_RoleToUser_UserId'
GO
ALTER TABLE [Mapping].[RoleToUser] ADD CONSTRAINT
	FK_RoleToUser_RoleId FOREIGN KEY
	(
	RoleId
	) REFERENCES [Administration.Role].[Role]
	(
	RoleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[RoleToUser].RoleId to [Administration.Role].[Role].RoleId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'RoleToUser', N'CONSTRAINT', N'FK_RoleToUser_RoleId'
GO
ALTER TABLE [Mapping].[RoleToUser] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
