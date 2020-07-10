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
USE [EMaaS]
GO
CREATE TABLE [Information].[OrdinalNumber]
	(
	OrdinalNumberId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,	
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	OrdinalNumberDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	DF_OrdinalNumber_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	DF_OrdinalNumber_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	DF_OrdinalNumber_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	PK_OrdinalNumber PRIMARY KEY CLUSTERED 
	(
	OrdinalNumberId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	FK_OrdinalNumber_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[OrdinalNumber].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'OrdinalNumber', N'CONSTRAINT', N'FK_OrdinalNumber_CreatedByUserId'
GO
ALTER TABLE [Information].[OrdinalNumber] ADD CONSTRAINT
	FK_OrdinalNumber_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[OrdinalNumber].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'OrdinalNumber', N'CONSTRAINT', N'FK_OrdinalNumber_SourceId'
GO
ALTER TABLE [Information].[OrdinalNumber] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
