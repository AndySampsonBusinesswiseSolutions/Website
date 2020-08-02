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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[EmailToVerification]') AND type in (N'U'))
DROP TABLE [Mapping].[EmailToVerification]
GO
CREATE TABLE [Mapping].[EmailToVerification]
	(
	EmailToVerificationId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	EmailId BIGINT NOT NULL,
	VerificationId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	PK_EmailToVerification PRIMARY KEY CLUSTERED 
	(
	EmailToVerificationId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	DF_EmailToVerification_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	DF_EmailToVerification_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	DF_EmailToVerification_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	FK_EmailToVerification_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[EmailToVerification].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'EmailToVerification', N'CONSTRAINT', N'FK_EmailToVerification_CreatedByUserId'
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	FK_EmailToVerification_VerificationId FOREIGN KEY
	(
	VerificationId
	) REFERENCES [Administration.User].[Verification]
	(
	VerificationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[EmailToVerification].VerificationId to [Administration.User].[Verification].VerificationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'EmailToVerification', N'CONSTRAINT', N'FK_EmailToVerification_VerificationId'
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	FK_EmailToVerification_EmailId FOREIGN KEY
	(
	EmailId
	) REFERENCES [Communication].[Email]
	(
	EmailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[EmailToVerification].EmailId to [Communication].[Email].EmailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'EmailToVerification', N'CONSTRAINT', N'FK_EmailToVerification_EmailId'
GO
ALTER TABLE [Mapping].[EmailToVerification] ADD CONSTRAINT
	FK_EmailToVerification_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[EmailToVerification].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'EmailToVerification', N'CONSTRAINT', N'FK_EmailToVerification_SourceId'
GO
ALTER TABLE [Mapping].[EmailToVerification] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
