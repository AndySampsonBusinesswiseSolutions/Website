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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[LoginToVerification]') AND type in (N'U'))
DROP TABLE [Mapping].[LoginToVerification]
GO
CREATE TABLE [Mapping].[LoginToVerification]
	(
	LoginToVerificationId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LoginId BIGINT NOT NULL,
	VerificationId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	PK_LoginToVerification PRIMARY KEY CLUSTERED 
	(
	LoginToVerificationId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	DF_LoginToVerification_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	DF_LoginToVerification_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	DF_LoginToVerification_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	FK_LoginToVerification_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToVerification].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToVerification', N'CONSTRAINT', N'FK_LoginToVerification_CreatedByUserId'
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	FK_LoginToVerification_VerificationId FOREIGN KEY
	(
	VerificationId
	) REFERENCES [Administration.User].[Verification]
	(
	VerificationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToVerification].VerificationId to [Administration.User].[Verification].VerificationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToVerification', N'CONSTRAINT', N'FK_LoginToVerification_VerificationId'
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	FK_LoginToVerification_LoginId FOREIGN KEY
	(
	LoginId
	) REFERENCES [Administration.User].[Login]
	(
	LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToVerification].LoginId to [Administration.User].[Login].LoginId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToVerification', N'CONSTRAINT', N'FK_LoginToVerification_LoginId'
GO
ALTER TABLE [Mapping].[LoginToVerification] ADD CONSTRAINT
	FK_LoginToVerification_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LoginToVerification].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LoginToVerification', N'CONSTRAINT', N'FK_LoginToVerification_SourceId'
GO
ALTER TABLE [Mapping].[LoginToVerification] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
