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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[TradeDetailToVolumeUnit]') AND type in (N'U'))
DROP TABLE [Mapping].[TradeDetailToVolumeUnit]
GO
CREATE TABLE [Mapping].[TradeDetailToVolumeUnit]
	(
	TradeDetailToVolumeUnitId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TradeDetailId BIGINT NOT NULL,
	VolumeUnitId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	PK_TradeDetailToVolumeUnit PRIMARY KEY CLUSTERED 
	(
	TradeDetailToVolumeUnitId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	DF_TradeDetailToVolumeUnit_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	DF_TradeDetailToVolumeUnit_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	DF_TradeDetailToVolumeUnit_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	FK_TradeDetailToVolumeUnit_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeDetailToVolumeUnit].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeDetailToVolumeUnit', N'CONSTRAINT', N'FK_TradeDetailToVolumeUnit_CreatedByUserId'
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	FK_TradeDetailToVolumeUnit_VolumeUnitId FOREIGN KEY
	(
	VolumeUnitId
	) REFERENCES [Information].[VolumeUnit]
	(
	VolumeUnitId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeDetailToVolumeUnit].VolumeUnitId to [Information].[VolumeUnit].VolumeUnitId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeDetailToVolumeUnit', N'CONSTRAINT', N'FK_TradeDetailToVolumeUnit_VolumeUnitId'
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	FK_TradeDetailToVolumeUnit_TradeDetailId FOREIGN KEY
	(
	TradeDetailId
	) REFERENCES [Customer].[TradeDetail]
	(
	TradeDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeDetailToVolumeUnit].TradeDetailId to [Customer].[TradeDetail].TradeDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeDetailToVolumeUnit', N'CONSTRAINT', N'FK_TradeDetailToVolumeUnit_TradeDetailId'
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] ADD CONSTRAINT
	FK_TradeDetailToVolumeUnit_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeDetailToVolumeUnit].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeDetailToVolumeUnit', N'CONSTRAINT', N'FK_TradeDetailToVolumeUnit_SourceId'
GO
ALTER TABLE [Mapping].[TradeDetailToVolumeUnit] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
