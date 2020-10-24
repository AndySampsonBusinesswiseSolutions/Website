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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToContractType]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToContractType]
GO
CREATE TABLE [Mapping].[ContractToContractType]
	(
	ContractToContractTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	ContractTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	PK_ContractToContractType PRIMARY KEY CLUSTERED 
	(
	ContractToContractTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	DF_ContractToContractType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	DF_ContractToContractType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	DF_ContractToContractType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	FK_ContractToContractType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractType', N'CONSTRAINT', N'FK_ContractToContractType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	FK_ContractToContractType_ContractTypeId FOREIGN KEY
	(
	ContractTypeId
	) REFERENCES [Information].[ContractType]
	(
	ContractTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractType].ContractTypeId to [Information].[ContractType].ContractTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractType', N'CONSTRAINT', N'FK_ContractToContractType_ContractTypeId'
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	FK_ContractToContractType_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractType].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractType', N'CONSTRAINT', N'FK_ContractToContractType_ContractId'
GO
ALTER TABLE [Mapping].[ContractToContractType] ADD CONSTRAINT
	FK_ContractToContractType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToContractType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToContractType', N'CONSTRAINT', N'FK_ContractToContractType_SourceId'
GO
ALTER TABLE [Mapping].[ContractToContractType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
