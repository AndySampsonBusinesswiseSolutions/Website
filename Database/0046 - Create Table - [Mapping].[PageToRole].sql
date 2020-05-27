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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[PageToRole]') AND type in (N'U'))
DROP TABLE [Mapping].[PageToRole]
GO
CREATE TABLE [Mapping].[PageToRole]
	(
	PageToRoleId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	PageId bigint NOT NULL,
	RoleId bigint NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	PK_PageToRole PRIMARY KEY CLUSTERED 
	(
	PageToRoleId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	DF_PageToRole_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	DF_PageToRole_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	DF_PageToRole_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	FK_PageToRole_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToRole].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToRole', N'CONSTRAINT', N'FK_PageToRole_CreatedByUserId'
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	FK_PageToRole_RoleId FOREIGN KEY
	(
	RoleId
	) REFERENCES [Administration.Role].[Role]
	(
	RoleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToRole].RoleId to [Administration.Role].[Role].RoleId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToRole', N'CONSTRAINT', N'FK_PageToRole_RoleId'
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	FK_PageToRole_PageId FOREIGN KEY
	(
	PageId
	) REFERENCES [System].[Page]
	(
	PageId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToRole].PageId to [System].[Page].PageId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToRole', N'CONSTRAINT', N'FK_PageToRole_PageId'
GO
ALTER TABLE [Mapping].[PageToRole] ADD CONSTRAINT
	FK_PageToRole_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToRole].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToRole', N'CONSTRAINT', N'FK_PageToRole_SourceId'
GO
ALTER TABLE [Mapping].[PageToRole] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
