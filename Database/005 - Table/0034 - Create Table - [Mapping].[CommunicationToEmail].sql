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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommunicationToEmail]') AND type in (N'U'))
DROP TABLE [Mapping].[CommunicationToEmail]
GO
CREATE TABLE [Mapping].[CommunicationToEmail]
	(
	CommunicationToEmailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	CommunicationId bigint NOT NULL,
	EmailId bigint NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	PK_CommunicationToEmail PRIMARY KEY CLUSTERED 
	(
	CommunicationToEmailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	DF_CommunicationToEmail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	DF_CommunicationToEmail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	DF_CommunicationToEmail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	FK_CommunicationToEmail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommunicationToEmail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommunicationToEmail', N'CONSTRAINT', N'FK_CommunicationToEmail_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	FK_CommunicationToEmail_EmailId FOREIGN KEY
	(
	EmailId
	) REFERENCES [Communication].[Email]
	(
	EmailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommunicationToEmail].EmailId to [Administration.User].[Email].EmailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommunicationToEmail', N'CONSTRAINT', N'FK_CommunicationToEmail_EmailId'
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	FK_CommunicationToEmail_CommunicationId FOREIGN KEY
	(
	CommunicationId
	) REFERENCES [Communication].[Communication]
	(
	CommunicationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommunicationToEmail].CommunicationId to [Communication].[Communication].CommunicationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommunicationToEmail', N'CONSTRAINT', N'FK_CommunicationToEmail_CommunicationId'
GO
ALTER TABLE [Mapping].[CommunicationToEmail] ADD CONSTRAINT
	FK_CommunicationToEmail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommunicationToEmail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommunicationToEmail', N'CONSTRAINT', N'FK_CommunicationToEmail_SourceId'
GO
ALTER TABLE [Mapping].[CommunicationToEmail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
