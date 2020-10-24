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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToReferenceVolume]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToReferenceVolume]
GO
CREATE TABLE [Mapping].[ContractToReferenceVolume]
	(
	ContractToReferenceVolumeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	ReferenceVolumeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	PK_ContractToReferenceVolume PRIMARY KEY CLUSTERED 
	(
	ContractToReferenceVolumeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	DF_ContractToReferenceVolume_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	DF_ContractToReferenceVolume_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	DF_ContractToReferenceVolume_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	FK_ContractToReferenceVolume_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToReferenceVolume].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToReferenceVolume', N'CONSTRAINT', N'FK_ContractToReferenceVolume_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	FK_ContractToReferenceVolume_ReferenceVolumeId FOREIGN KEY
	(
	ReferenceVolumeId
	) REFERENCES [Customer].[ReferenceVolume]
	(
	ReferenceVolumeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToReferenceVolume].ReferenceVolumeId to [Customer].[ReferenceVolume].ReferenceVolumeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToReferenceVolume', N'CONSTRAINT', N'FK_ContractToReferenceVolume_ReferenceVolumeId'
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	FK_ContractToReferenceVolume_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToReferenceVolume].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToReferenceVolume', N'CONSTRAINT', N'FK_ContractToReferenceVolume_ContractId'
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] ADD CONSTRAINT
	FK_ContractToReferenceVolume_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToReferenceVolume].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToReferenceVolume', N'CONSTRAINT', N'FK_ContractToReferenceVolume_SourceId'
GO
ALTER TABLE [Mapping].[ContractToReferenceVolume] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
