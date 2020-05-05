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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Communication].[Communication]') AND type in (N'U'))
DROP TABLE [Communication].[Communication]
GO
CREATE TABLE [Communication].[Communication]
	(
	CommunicationId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	CommunicationTypeId bigint NOT NULL,
	CommunicationTypeEntityId bigint NOT NULL
	)  ON Communication
GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	PK_Communication PRIMARY KEY CLUSTERED 
	(
	CommunicationId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Communication

GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	DF_Communication_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	DF_Communication_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	DF_Communication_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	FK_Communication_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[Communication].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'Communication', N'CONSTRAINT', N'FK_Communication_CreatedByUserId'
GO
ALTER TABLE [Communication].[Communication] ADD CONSTRAINT
	FK_Communication_CommunicationTypeId FOREIGN KEY
	(
	CommunicationTypeId
	) REFERENCES [Communication].[CommunicationType]
	(
	CommunicationTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[Communication].CommunicationTypeId to [Communication].[CommunicationType].CommunicationTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'Communication', N'CONSTRAINT', N'FK_Communication_CommunicationTypeId'
GO
ALTER TABLE [Communication].[Communication] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
