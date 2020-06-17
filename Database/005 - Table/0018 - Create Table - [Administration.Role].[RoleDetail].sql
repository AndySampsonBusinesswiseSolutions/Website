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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Administration.Role].[RoleDetail]') AND type in (N'U'))
DROP TABLE [Administration.Role].[RoleDetail]
GO
CREATE TABLE [Administration.Role].[RoleDetail]
	(
	RoleDetailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	RoleId bigint NOT NULL,
	RoleAttributeId bigint NOT NULL,
	RoleDetailDescription varchar(200) NOT NULL
	)  ON [Administration]
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	PK_RoleDetail PRIMARY KEY CLUSTERED 
	(
	RoleDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Administration]

GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	DF_RoleDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	DF_RoleDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	DF_RoleDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	FK_RoleDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleDetail', N'CONSTRAINT', N'FK_RoleDetail_CreatedByUserId'
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	FK_RoleDetail_RoleId FOREIGN KEY
	(
	RoleId
	) REFERENCES [Administration.Role].[Role]
	(
	RoleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleDetail].RoleId to [Administration.Role].[Role].RoleId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleDetail', N'CONSTRAINT', N'FK_RoleDetail_RoleId'
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	FK_RoleDetail_RoleAttributeId FOREIGN KEY
	(
	RoleAttributeId
	) REFERENCES [Administration.Role].[RoleAttribute]
	(
	RoleAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleDetail].RoleAttributeId to [Administration.Role].[RoleAttribute].RoleAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleDetail', N'CONSTRAINT', N'FK_RoleDetail_RoleAttributeId'
GO
ALTER TABLE [Administration.Role].[RoleDetail] ADD CONSTRAINT
	FK_RoleDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.Role].[RoleDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.Role', N'TABLE', N'RoleDetail', N'CONSTRAINT', N'FK_RoleDetail_SourceId'
GO
ALTER TABLE [Administration.Role].[RoleDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
