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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Communication].[Email]') AND type in (N'U'))
DROP TABLE [Communication].[Email]
GO
CREATE TABLE [Communication].[Email]
	(
	EmailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	CommunicationId bigint NOT NULL
	)  ON Communication
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	PK_Email PRIMARY KEY CLUSTERED 
	(
	EmailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Communication

GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	DF_Email_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	DF_Email_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	DF_Email_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	FK_Email_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[Email].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'Email', N'CONSTRAINT', N'FK_Email_CreatedByUserId'
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	FK_Email_CommunicationId FOREIGN KEY
	(
	CommunicationId
	) REFERENCES [Communication].[Communication]
	(
	CommunicationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[Email].CommunicationId to [Communication].[Communication].CommunicationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'Email', N'CONSTRAINT', N'FK_Email_CommunicationId'
GO
ALTER TABLE [Communication].[Email] ADD CONSTRAINT
	FK_Email_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[Email].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'Email', N'CONSTRAINT', N'FK_Email_SourceId'
GO
ALTER TABLE [Communication].[Email] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
