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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[AssetToSubMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[AssetToSubMeter]
GO
CREATE TABLE [Mapping].[AssetToSubMeter]
	(
	AssetToSubMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	AssetId BIGINT NOT NULL,
	SubMeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	PK_AssetToSubMeter PRIMARY KEY CLUSTERED 
	(
	AssetToSubMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	DF_AssetToSubMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	DF_AssetToSubMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	DF_AssetToSubMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	FK_AssetToSubMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AssetToSubMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AssetToSubMeter', N'CONSTRAINT', N'FK_AssetToSubMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	FK_AssetToSubMeter_SubMeterId FOREIGN KEY
	(
	SubMeterId
	) REFERENCES [Customer].[SubMeter]
	(
	SubMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AssetToSubMeter].SubMeterId to [Customer].[SubMeter].SubMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AssetToSubMeter', N'CONSTRAINT', N'FK_AssetToSubMeter_SubMeterId'
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	FK_AssetToSubMeter_AssetId FOREIGN KEY
	(
	AssetId
	) REFERENCES [Customer].[Asset]
	(
	AssetId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AssetToSubMeter].AssetId to [Customer].[Asset].AssetId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AssetToSubMeter', N'CONSTRAINT', N'FK_AssetToSubMeter_AssetId'
GO
ALTER TABLE [Mapping].[AssetToSubMeter] ADD CONSTRAINT
	FK_AssetToSubMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AssetToSubMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AssetToSubMeter', N'CONSTRAINT', N'FK_AssetToSubMeter_SourceId'
GO
ALTER TABLE [Mapping].[AssetToSubMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
