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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[BasketToContractMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[BasketToContractMeter]
GO
CREATE TABLE [Mapping].[BasketToContractMeter]
	(
	BasketToContractMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	BasketId BIGINT NOT NULL,
	ContractMeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	PK_BasketToContractMeter PRIMARY KEY CLUSTERED 
	(
	BasketToContractMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	DF_BasketToContractMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	DF_BasketToContractMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	DF_BasketToContractMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	FK_BasketToContractMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[BasketToContractMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'BasketToContractMeter', N'CONSTRAINT', N'FK_BasketToContractMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	FK_BasketToContractMeter_ContractMeterId FOREIGN KEY
	(
	ContractMeterId
	) REFERENCES [Customer].[ContractMeter]
	(
	ContractMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[BasketToContractMeter].ContractMeterId to [Customer].[ContractMeter].ContractMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'BasketToContractMeter', N'CONSTRAINT', N'FK_BasketToContractMeter_ContractMeterId'
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	FK_BasketToContractMeter_BasketId FOREIGN KEY
	(
	BasketId
	) REFERENCES [Customer].[Basket]
	(
	BasketId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[BasketToContractMeter].BasketId to [Customer].[Basket].BasketId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'BasketToContractMeter', N'CONSTRAINT', N'FK_BasketToContractMeter_BasketId'
GO
ALTER TABLE [Mapping].[BasketToContractMeter] ADD CONSTRAINT
	FK_BasketToContractMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[BasketToContractMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'BasketToContractMeter', N'CONSTRAINT', N'FK_BasketToContractMeter_SourceId'
GO
ALTER TABLE [Mapping].[BasketToContractMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
