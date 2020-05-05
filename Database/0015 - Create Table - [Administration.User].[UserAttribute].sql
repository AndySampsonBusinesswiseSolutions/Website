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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.User].[UserAttribute]') AND type in (N'U'))
DROP TABLE [Administration.User].[UserAttribute]
GO
CREATE TABLE [Administration.User].[UserAttribute]
	(
	UserAttributeId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	UserAttributeDescription varchar(200) NOT NULL
	)  ON Administration
GO
ALTER TABLE [Administration.User].[UserAttribute] ADD CONSTRAINT
	PK_UserAttribute PRIMARY KEY CLUSTERED 
	(
	UserAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Administration

GO
ALTER TABLE [Administration.User].[UserAttribute] ADD CONSTRAINT
	DF_UserAttribute_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.User].[UserAttribute] ADD CONSTRAINT
	DF_UserAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.User].[UserAttribute] ADD CONSTRAINT
	DF_UserAttribute_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.User].[UserAttribute] ADD CONSTRAINT
	FK_UserAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[UserAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'UserAttribute', N'CONSTRAINT', N'FK_UserAttribute_CreatedByUserId'
GO
ALTER TABLE [Administration.User].[UserAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
