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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToSupplier]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToSupplier]
GO
CREATE TABLE [Mapping].[ContractToSupplier]
	(
	ContractToSupplierId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	SupplierId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	PK_ContractToSupplier PRIMARY KEY CLUSTERED 
	(
	ContractToSupplierId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	DF_ContractToSupplier_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	DF_ContractToSupplier_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	DF_ContractToSupplier_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	FK_ContractToSupplier_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToSupplier].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToSupplier', N'CONSTRAINT', N'FK_ContractToSupplier_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	FK_ContractToSupplier_SupplierId FOREIGN KEY
	(
	SupplierId
	) REFERENCES [Supplier].[Supplier]
	(
	SupplierId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToSupplier].SupplierId to [Supplier].[Supplier].SupplierId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToSupplier', N'CONSTRAINT', N'FK_ContractToSupplier_SupplierId'
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	FK_ContractToSupplier_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToSupplier].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToSupplier', N'CONSTRAINT', N'FK_ContractToSupplier_ContractId'
GO
ALTER TABLE [Mapping].[ContractToSupplier] ADD CONSTRAINT
	FK_ContractToSupplier_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToSupplier].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToSupplier', N'CONSTRAINT', N'FK_ContractToSupplier_SourceId'
GO
ALTER TABLE [Mapping].[ContractToSupplier] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
