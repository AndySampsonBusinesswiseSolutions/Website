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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Communication].[CommunicationAttribute]') AND type in (N'U'))
DROP TABLE [Communication].[CommunicationAttribute]
GO
CREATE TABLE [Communication].[CommunicationAttribute]
	(
	CommunicationAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommunicationAttributeDescription VARCHAR(200) NOT NULL
	)  ON [Communication]
GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	PK_CommunicationAttribute PRIMARY KEY CLUSTERED 
	(
	CommunicationAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Communication]

GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	DF_CommunicationAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	DF_CommunicationAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	DF_CommunicationAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	FK_CommunicationAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationAttribute', N'CONSTRAINT', N'FK_CommunicationAttribute_CreatedByUserId'
GO
ALTER TABLE [Communication].[CommunicationAttribute] ADD CONSTRAINT
	FK_CommunicationAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationAttribute', N'CONSTRAINT', N'FK_CommunicationAttribute_SourceId'
GO
ALTER TABLE [Communication].[CommunicationAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
