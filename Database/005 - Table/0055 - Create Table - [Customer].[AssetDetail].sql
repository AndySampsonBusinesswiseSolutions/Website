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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[AssetDetail]') AND type in (N'U'))
DROP TABLE [Customer].[AssetDetail]
GO
CREATE TABLE [Customer].[AssetDetail]
	(
	AssetDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	AssetId BIGINT NOT NULL,
	AssetAttributeId BIGINT NOT NULL,
	AssetDetailDescription VARCHAR(200) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	PK_AssetDetail PRIMARY KEY CLUSTERED 
	(
	AssetDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	DF_AssetDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	DF_AssetDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	DF_AssetDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	FK_AssetDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetDetail', N'CONSTRAINT', N'FK_AssetDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	FK_AssetDetail_AssetId FOREIGN KEY
	(
	AssetId
	) REFERENCES [Customer].[Asset]
	(
	AssetId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetDetail].AssetId to [Customer].[Asset].AssetId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetDetail', N'CONSTRAINT', N'FK_AssetDetail_AssetId'
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	FK_AssetDetail_AssetAttributeId FOREIGN KEY
	(
	AssetAttributeId
	) REFERENCES [Customer].[AssetAttribute]
	(
	AssetAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetDetail].AssetAttributeId to [Customer].[AssetAttribute].AssetAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetDetail', N'CONSTRAINT', N'FK_AssetDetail_AssetAttributeId'
GO
ALTER TABLE [Customer].[AssetDetail] ADD CONSTRAINT
	FK_AssetDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[AssetDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'AssetDetail', N'CONSTRAINT', N'FK_AssetDetail_SourceId'
GO
ALTER TABLE [Customer].[AssetDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
