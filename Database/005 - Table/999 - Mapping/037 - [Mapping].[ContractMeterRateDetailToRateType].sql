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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractMeterRateDetailToRateType]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractMeterRateDetailToRateType]
GO
CREATE TABLE [Mapping].[ContractMeterRateDetailToRateType]
	(
	ContractMeterRateDetailToRateTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractMeterRateDetailId BIGINT NOT NULL,
	RateTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	PK_ContractMeterRateDetailToRateType PRIMARY KEY CLUSTERED 
	(
	ContractMeterRateDetailToRateTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	DF_ContractMeterRateDetailToRateType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	DF_ContractMeterRateDetailToRateType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	DF_ContractMeterRateDetailToRateType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	FK_ContractMeterRateDetailToRateType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateDetailToRateType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateDetailToRateType', N'CONSTRAINT', N'FK_ContractMeterRateDetailToRateType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	FK_ContractMeterRateDetailToRateType_RateTypeId FOREIGN KEY
	(
	RateTypeId
	) REFERENCES [Information].[RateType]
	(
	RateTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateDetailToRateType].RateTypeId to [Information].[RateType].RateTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateDetailToRateType', N'CONSTRAINT', N'FK_ContractMeterRateDetailToRateType_RateTypeId'
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	FK_ContractMeterRateDetailToRateType_ContractMeterRateDetailId FOREIGN KEY
	(
	ContractMeterRateDetailId
	) REFERENCES [Customer].[ContractMeterRateDetail]
	(
	ContractMeterRateDetailId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateDetailToRateType].ContractMeterRateDetailId to [Customer].[ContractMeterRateDetail].ContractMeterRateDetailId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateDetailToRateType', N'CONSTRAINT', N'FK_ContractMeterRateDetailToRateType_ContractMeterRateDetailId'
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] ADD CONSTRAINT
	FK_ContractMeterRateDetailToRateType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateDetailToRateType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateDetailToRateType', N'CONSTRAINT', N'FK_ContractMeterRateDetailToRateType_SourceId'
GO
ALTER TABLE [Mapping].[ContractMeterRateDetailToRateType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
