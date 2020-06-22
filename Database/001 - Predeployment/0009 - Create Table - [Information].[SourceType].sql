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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[SourceType]') AND type in (N'U'))
DROP TABLE [Information].[SourceType]
GO
CREATE TABLE [Information].[SourceType]
	(
	SourceTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NULL,
	SourceTypeDescription VARCHAR(200) NOT NULL,
	)  ON Information
GO
ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	PK_SourceType PRIMARY KEY CLUSTERED 
	(
	SourceTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Information

GO
ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	DF_SourceType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	DF_SourceType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	DF_SourceType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	FK_SourceType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceType', N'CONSTRAINT', N'FK_SourceType_CreatedByUserId'
ALTER TABLE [Information].[SourceType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
