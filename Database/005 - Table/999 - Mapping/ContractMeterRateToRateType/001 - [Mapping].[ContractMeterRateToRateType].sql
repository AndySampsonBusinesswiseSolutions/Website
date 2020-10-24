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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractMeterRateToRateType]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractMeterRateToRateType]
GO
CREATE TABLE [Mapping].[ContractMeterRateToRateType]
	(
	ContractMeterRateToRateTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractMeterRateId BIGINT NOT NULL,
	RateTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	PK_ContractMeterRateToRateType PRIMARY KEY CLUSTERED 
	(
	ContractMeterRateToRateTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractMeterRateToRateType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractMeterRateToRateType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractMeterRateToRateType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractMeterRateToRateType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateToRateType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractMeterRateToRateType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractMeterRateToRateType_RateTypeId FOREIGN KEY
	(
	RateTypeId
	) REFERENCES [Information].[RateType]
	(
	RateTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateToRateType].RateTypeId to [Information].[RateType].RateTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractMeterRateToRateType_RateTypeId'
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractMeterRateToRateType_ContractMeterRateId FOREIGN KEY
	(
	ContractMeterRateId
	) REFERENCES [Customer].[ContractMeterRate]
	(
	ContractMeterRateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateToRateType].ContractMeterRateId to [Customer].[ContractMeterRate].ContractMeterRateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractMeterRateToRateType_ContractMeterRateId'
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractMeterRateToRateType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractMeterRateToRateType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractMeterRateToRateType_SourceId'
GO
ALTER TABLE [Mapping].[ContractMeterRateToRateType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
