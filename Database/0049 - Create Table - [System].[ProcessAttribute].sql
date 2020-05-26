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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ProcessAttribute]') AND type in (N'U'))
DROP TABLE [System].[ProcessAttribute]
GO
CREATE TABLE [System].[ProcessAttribute]
	(
	ProcessAttributeId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	ProcessAttributeDescription varchar(200) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ProcessAttribute] ADD CONSTRAINT
	PK_ProcessAttribute PRIMARY KEY CLUSTERED 
	(
	ProcessAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ProcessAttribute] ADD CONSTRAINT
	DF_ProcessAttribute_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ProcessAttribute] ADD CONSTRAINT
	DF_ProcessAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ProcessAttribute] ADD CONSTRAINT
	DF_ProcessAttribute_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ProcessAttribute] ADD CONSTRAINT
	FK_ProcessAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessAttribute', N'CONSTRAINT', N'FK_ProcessAttribute_CreatedByUserId'
GO
ALTER TABLE [System].[ProcessAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
