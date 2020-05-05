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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageDetail]') AND type in (N'U'))
DROP TABLE [System].[PageDetail]
GO
CREATE TABLE [System].[PageDetail]
	(
	PageDetailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	PageId bigint NOT NULL,
	PageAttributeId bigint NOT NULL,
	PageDetailDescription varchar(200) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	PK_PageDetail PRIMARY KEY CLUSTERED 
	(
	PageDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	DF_PageDetail_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	DF_PageDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	DF_PageDetail_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	FK_PageDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageDetail', N'CONSTRAINT', N'FK_PageDetail_CreatedByUserId'
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	FK_PageDetail_PageId FOREIGN KEY
	(
	PageId
	) REFERENCES [System].[Page]
	(
	PageId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageDetail].PageId to [System].[Page].PageId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageDetail', N'CONSTRAINT', N'FK_PageDetail_PageId'
GO
ALTER TABLE [System].[PageDetail] ADD CONSTRAINT
	FK_PageDetail_PageAttributeId FOREIGN KEY
	(
	PageAttributeId
	) REFERENCES [System].[PageAttribute]
	(
	PageAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageDetail].PageAttributeId to [System].[PageAttribute].PageAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageDetail', N'CONSTRAINT', N'FK_PageDetail_PageAttributeId'
GO
ALTER TABLE [System].[PageDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
