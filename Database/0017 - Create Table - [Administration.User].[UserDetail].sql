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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.User].[UserDetail]') AND type in (N'U'))
DROP TABLE [Administration.User].[UserDetail]
GO
CREATE TABLE [Administration.User].[UserDetail]
	(
	UserDetailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	UserId bigint NOT NULL,
	UserAttributeId bigint NOT NULL,
	UserDetailDescription varchar(200) NOT NULL
	)  ON Administration
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	PK_UserDetail PRIMARY KEY CLUSTERED 
	(
	UserDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Administration

GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	DF_UserDetail_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	DF_UserDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	DF_UserDetail_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	FK_UserDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[UserDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'UserDetail', N'CONSTRAINT', N'FK_UserDetail_CreatedByUserId'
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	FK_UserDetail_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[UserDetail].UserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'UserDetail', N'CONSTRAINT', N'FK_UserDetail_UserId'
GO
ALTER TABLE [Administration.User].[UserDetail] ADD CONSTRAINT
	FK_UserDetail_UserAttributeId FOREIGN KEY
	(
	UserAttributeId
	) REFERENCES [Administration.User].[UserAttribute]
	(
	UserAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[UserDetail].UserAttributeId to [Administration.User].[UserAttribute].UserAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'UserDetail', N'CONSTRAINT', N'FK_UserDetail_UserAttributeId'
GO
ALTER TABLE [Administration.User].[UserDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
