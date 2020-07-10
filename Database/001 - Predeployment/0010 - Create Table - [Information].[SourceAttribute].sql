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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[SourceAttribute]') AND type in (N'U'))
DROP TABLE [Information].[SourceAttribute]
GO
CREATE TABLE [Information].[SourceAttribute]
	(
	SourceAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NULL,
	SourceAttributeDescription VARCHAR(255) NOT NULL,
	)  ON [Information]
GO
ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	PK_SourceAttribute PRIMARY KEY CLUSTERED 
	(
	SourceAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	DF_SourceAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	DF_SourceAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	DF_SourceAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	FK_SourceAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceAttribute', N'CONSTRAINT', N'FK_SourceAttribute_CreatedByUserId'
ALTER TABLE [Information].[SourceAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
