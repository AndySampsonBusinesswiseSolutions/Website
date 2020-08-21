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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType]
GO
CREATE TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType]
	(
	ContractToMeterToContractMeterToProductToContractMeterRateToRateTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractToMeterToContractMeterToProductId BIGINT NOT NULL,
	ContractMeterRateToRateTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	PK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType PRIMARY KEY CLUSTERED 
	(
	ContractToMeterToContractMeterToProductToContractMeterRateToRateTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProductToContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_ContractMeterRateToRateTypeId FOREIGN KEY
	(
	ContractMeterRateToRateTypeId
	) REFERENCES [Mapping].[ContractMeterRateToRateType]
	(
	ContractMeterRateToRateTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType].ContractMeterRateToRateTypeId to [Mapping].[ContractMeterRateToRateType].ContractMeterRateToRateTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProductToContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_ContractMeterRateToRateTypeId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_ContractToMeterToContractMeterToProductId FOREIGN KEY
	(
	ContractToMeterToContractMeterToProductId
	) REFERENCES [Mapping].[ContractToMeterToContractMeterToProduct]
	(
	ContractToMeterToContractMeterToProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType].ContractToMeterToContractMeterToProductId to [Mapping].[ContractToMeterToContractMeterToProduct].ContractToMeterToContractMeterToProductId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProductToContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_ContractToMeterToContractMeterToProductId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProductToContractMeterRateToRateType', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProductToContractMeterRateToRateType_SourceId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
