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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToMeterToContractMeterToProduct]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToMeterToContractMeterToProduct]
GO
CREATE TABLE [Mapping].[ContractToMeterToContractMeterToProduct]
	(
	ContractToMeterToContractMeterToProductId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractToMeterId BIGINT NOT NULL,
	ContractMeterToProductId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	PK_ContractToMeterToContractMeterToProduct PRIMARY KEY CLUSTERED 
	(
	ContractToMeterToContractMeterToProductId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProduct_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProduct_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	DF_ContractToMeterToContractMeterToProduct_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProduct_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProduct].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProduct', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProduct_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProduct_ContractMeterToProductId FOREIGN KEY
	(
	ContractMeterToProductId
	) REFERENCES [Mapping].[ContractMeterToProduct]
	(
	ContractMeterToProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProduct].ContractMeterToProductId to [Mapping].[ContractMeterToProduct].ContractMeterToProductId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProduct', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProduct_ContractMeterToProductId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProduct_ContractToMeterId FOREIGN KEY
	(
	ContractToMeterId
	) REFERENCES [Mapping].[ContractToMeter]
	(
	ContractToMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProduct].ContractToMeterId to [Mapping].[ContractToMeter].ContractToMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProduct', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProduct_ContractToMeterId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] ADD CONSTRAINT
	FK_ContractToMeterToContractMeterToProduct_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeterToContractMeterToProduct].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeterToContractMeterToProduct', N'CONSTRAINT', N'FK_ContractToMeterToContractMeterToProduct_SourceId'
GO
ALTER TABLE [Mapping].[ContractToMeterToContractMeterToProduct] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
