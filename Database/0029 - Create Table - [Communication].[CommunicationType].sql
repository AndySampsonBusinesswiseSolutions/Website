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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Communication].[CommunicationType]') AND type in (N'U'))
DROP TABLE [Communication].[CommunicationType]
GO
CREATE TABLE [Communication].[CommunicationType]
	(
	CommunicationTypeId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	CommunicationTypeDescription varchar(200) NOT NULL
	)  ON Communication
GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	PK_CommunicationType PRIMARY KEY CLUSTERED 
	(
	CommunicationTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Communication

GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	DF_CommunicationType_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	DF_CommunicationType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	DF_CommunicationType_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	FK_CommunicationType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationType', N'CONSTRAINT', N'FK_CommunicationType_CreatedByUserId'
GO
ALTER TABLE [Communication].[CommunicationType] ADD CONSTRAINT
	FK_CommunicationType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationType', N'CONSTRAINT', N'FK_CommunicationType_SourceId'
GO
ALTER TABLE [Communication].[CommunicationType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
