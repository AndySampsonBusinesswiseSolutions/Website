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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[APIAttribute]') AND type in (N'U'))
DROP TABLE [System].[APIAttribute]
GO
CREATE TABLE [System].[APIAttribute]
	(
	APIAttributeId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	APIAttributeDescription varchar(200) NOT NULL,
	AllowsMultipleActiveInstances BIT NOT NULL,
	)  ON [System]
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	PK_APIAttribute PRIMARY KEY CLUSTERED 
	(
	APIAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	DF_APIAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	DF_APIAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	DF_APIAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	DF_APIAttribute_AllowsMultipleActiveInstances DEFAULT 0 FOR AllowsMultipleActiveInstances
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	FK_APIAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIAttribute', N'CONSTRAINT', N'FK_APIAttribute_CreatedByUserId'
GO
ALTER TABLE [System].[APIAttribute] ADD CONSTRAINT
	FK_APIAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[APIAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'APIAttribute', N'CONSTRAINT', N'FK_APIAttribute_SourceId'
GO
ALTER TABLE [System].[APIAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
