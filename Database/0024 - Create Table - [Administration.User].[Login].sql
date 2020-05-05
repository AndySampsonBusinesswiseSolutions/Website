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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.User].[Login]') AND type in (N'U'))
DROP TABLE [Administration.User].[Login]
GO
CREATE TABLE [Administration.User].[Login]
	(
	LoginId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	LoginSuccessful bit NOT NULL
	)  ON Administration
GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	PK_Login PRIMARY KEY CLUSTERED 
	(
	LoginId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Administration

GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	DF_Login_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	DF_Login_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	DF_Login_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	DF_Login_LoginSuccessful DEFAULT 0 FOR LoginSuccessful
GO
ALTER TABLE [Administration.User].[Login] ADD CONSTRAINT
	FK_Login_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[Login].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'Login', N'CONSTRAINT', N'FK_Login_CreatedByUserId'
GO
ALTER TABLE [Administration.User].[Login] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
