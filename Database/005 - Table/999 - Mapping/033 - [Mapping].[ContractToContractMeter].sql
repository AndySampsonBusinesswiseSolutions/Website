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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToContractMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToContractMeter]
GO
CREATE TABLE [Mapping].[ContractToContractMeter]
	(
	ContractToContractMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	ContractMeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	PK_ContractToContractMeter PRIMARY KEY CLUSTERED 
	(
	ContractToContractMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	DF_ContractToContractMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	DF_ContractToContractMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	DF_ContractToContractMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	FK_ContractToContractMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractMeter', N'CONSTRAINT', N'FK_ContractToContractMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	FK_ContractToContractMeter_ContractMeterId FOREIGN KEY
	(
	ContractMeterId
	) REFERENCES [Customer].[ContractMeter]
	(
	ContractMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractMeter].ContractMeterId to [Customer].[ContractMeter].ContractMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractMeter', N'CONSTRAINT', N'FK_ContractToContractMeter_ContractMeterId'
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	FK_ContractToContractMeter_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractMeter].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractMeter', N'CONSTRAINT', N'FK_ContractToContractMeter_ContractId'
GO
ALTER TABLE [Mapping].[ContractToContractMeter] ADD CONSTRAINT
	FK_ContractToContractMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractMeter', N'CONSTRAINT', N'FK_ContractToContractMeter_SourceId'
GO
ALTER TABLE [Mapping].[ContractToContractMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
