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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Communication].[CommunicationDetail]') AND type in (N'U'))
DROP TABLE [Communication].[CommunicationDetail]
GO
CREATE TABLE [Communication].[CommunicationDetail]
	(
	CommunicationDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommunicationId BIGINT NOT NULL,
	CommunicationAttributeId BIGINT NOT NULL,
	CommunicationDetailDescription VARCHAR(200) NOT NULL
	)  ON [Communication]
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	PK_CommunicationDetail PRIMARY KEY CLUSTERED 
	(
	CommunicationDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Communication]

GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	DF_CommunicationDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	DF_CommunicationDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	DF_CommunicationDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	FK_CommunicationDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationDetail', N'CONSTRAINT', N'FK_CommunicationDetail_CreatedByUserId'
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	FK_CommunicationDetail_CommunicationId FOREIGN KEY
	(
	CommunicationId
	) REFERENCES [Communication].[Communication]
	(
	CommunicationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationDetail].CommunicationId to [Communication].[Communication].CommunicationId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationDetail', N'CONSTRAINT', N'FK_CommunicationDetail_CommunicationId'
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	FK_CommunicationDetail_CommunicationAttributeId FOREIGN KEY
	(
	CommunicationAttributeId
	) REFERENCES [Communication].[CommunicationAttribute]
	(
	CommunicationAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationDetail].CommunicationAttributeId to [Communication].[CommunicationAttribute].CommunicationAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationDetail', N'CONSTRAINT', N'FK_CommunicationDetail_CommunicationAttributeId'
GO
ALTER TABLE [Communication].[CommunicationDetail] ADD CONSTRAINT
	FK_CommunicationDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Communication].[CommunicationDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Communication', N'TABLE', N'CommunicationDetail', N'CONSTRAINT', N'FK_CommunicationDetail_SourceId'
GO
ALTER TABLE [Communication].[CommunicationDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
