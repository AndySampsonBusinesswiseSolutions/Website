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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageAttribute]') AND type in (N'U'))
DROP TABLE [System].[PageAttribute]
GO
CREATE TABLE [System].[PageAttribute]
	(
	PageAttributeId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	PageAttributeDescription varchar(200) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	PK_PageAttribute PRIMARY KEY CLUSTERED 
	(
	PageAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	DF_PageAttribute_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	DF_PageAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	DF_PageAttribute_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	FK_PageAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageAttribute', N'CONSTRAINT', N'FK_PageAttribute_CreatedByUserId'
GO
ALTER TABLE [System].[PageAttribute] ADD CONSTRAINT
	FK_PageAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageAttribute', N'CONSTRAINT', N'FK_PageAttribute_SourceId'
GO
ALTER TABLE [System].[PageAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
