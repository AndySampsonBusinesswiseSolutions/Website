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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ReferenceVolumeDetailToVolumeUnit]') AND type in (N'U'))
DROP TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit]
GO
CREATE TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit]
	(
	ReferenceVolumeDetailToVolumeUnitId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ReferenceVolumeDetailId BIGINT NOT NULL,
	VolumeUnitId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	PK_ReferenceVolumeDetailToVolumeUnit PRIMARY KEY CLUSTERED 
	(
	ReferenceVolumeDetailToVolumeUnitId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	DF_ReferenceVolumeDetailToVolumeUnit_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	DF_ReferenceVolumeDetailToVolumeUnit_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	DF_ReferenceVolumeDetailToVolumeUnit_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	FK_ReferenceVolumeDetailToVolumeUnit_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ReferenceVolumeDetailToVolumeUnit].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ReferenceVolumeDetailToVolumeUnit', N'CONSTRAINT', N'FK_ReferenceVolumeDetailToVolumeUnit_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	FK_ReferenceVolumeDetailToVolumeUnit_VolumeUnitId FOREIGN KEY
	(
	VolumeUnitId
	) REFERENCES [Information].[VolumeUnit]
	(
	VolumeUnitId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ReferenceVolumeDetailToVolumeUnit].VolumeUnitId to [Information].[VolumeUnit].VolumeUnitId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ReferenceVolumeDetailToVolumeUnit', N'CONSTRAINT', N'FK_ReferenceVolumeDetailToVolumeUnit_VolumeUnitId'
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	FK_ReferenceVolumeDetailToVolumeUnit_ReferenceVolumeDetailId FOREIGN KEY
	(
	ReferenceVolumeDetailId
	) REFERENCES [Customer].[ReferenceVolumeDetail]
	(
	ReferenceVolumeDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ReferenceVolumeDetailToVolumeUnit].ReferenceVolumeDetailId to [Customer].[ReferenceVolumeDetail].ReferenceVolumeDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ReferenceVolumeDetailToVolumeUnit', N'CONSTRAINT', N'FK_ReferenceVolumeDetailToVolumeUnit_ReferenceVolumeDetailId'
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] ADD CONSTRAINT
	FK_ReferenceVolumeDetailToVolumeUnit_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ReferenceVolumeDetailToVolumeUnit].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ReferenceVolumeDetailToVolumeUnit', N'CONSTRAINT', N'FK_ReferenceVolumeDetailToVolumeUnit_SourceId'
GO
ALTER TABLE [Mapping].[ReferenceVolumeDetailToVolumeUnit] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
