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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.Role].[RoleAttribute]') AND type in (N'U'))
DROP TABLE [Administration.Role].[RoleAttribute]
GO
CREATE TABLE [Administration.Role].[RoleAttribute]
	(
	RoleAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	RoleAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Administration]
GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	PK_RoleAttribute PRIMARY KEY CLUSTERED 
	(
	RoleAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Administration]

GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	DF_RoleAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	DF_RoleAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	DF_RoleAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	FK_RoleAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleAttribute', N'CONSTRAINT', N'FK_RoleAttribute_CreatedByUserId'
GO
ALTER TABLE [Administration.Role].[RoleAttribute] ADD CONSTRAINT
	FK_RoleAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleAttribute', N'CONSTRAINT', N'FK_RoleAttribute_SourceId'
GO
ALTER TABLE [Administration.Role].[RoleAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
