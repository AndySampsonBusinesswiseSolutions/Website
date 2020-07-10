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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.User].[Verification]') AND type in (N'U'))
DROP TABLE [Administration.User].[Verification]
GO
CREATE TABLE [Administration.User].[Verification]
	(
	VerificationId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	VerificationCode VARCHAR(255) NOT NULL,
	ExpiryDateTime DATETIME NOT NULL,
	)  ON [Administration]
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	PK_Verification PRIMARY KEY CLUSTERED 
	(
	VerificationId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Administration]

GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	DF_Verification_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	DF_Verification_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	DF_Verification_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	DF_Verification_ExpiryDateTime DEFAULT DATEADD(HOUR, 1, GETUTCDATE()) FOR ExpiryDateTime
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	FK_Verification_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[Verification].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'Verification', N'CONSTRAINT', N'FK_Verification_CreatedByUserId'
GO
ALTER TABLE [Administration.User].[Verification] ADD CONSTRAINT
	FK_Verification_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[Verification].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'Verification', N'CONSTRAINT', N'FK_Verification_SourceId'
GO
ALTER TABLE [Administration.User].[Verification] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
