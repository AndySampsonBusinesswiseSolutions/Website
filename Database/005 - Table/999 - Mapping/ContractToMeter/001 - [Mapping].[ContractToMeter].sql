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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ContractToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[ContractToMeter]
GO
CREATE TABLE [Mapping].[ContractToMeter]
	(
	ContractToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	PK_ContractToMeter PRIMARY KEY CLUSTERED 
	(
	ContractToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	DF_ContractToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	DF_ContractToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	DF_ContractToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	FK_ContractToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeter', N'CONSTRAINT', N'FK_ContractToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	FK_ContractToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeter].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeter', N'CONSTRAINT', N'FK_ContractToMeter_MeterId'
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	FK_ContractToMeter_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeter].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeter', N'CONSTRAINT', N'FK_ContractToMeter_ContractId'
GO
ALTER TABLE [Mapping].[ContractToMeter] ADD CONSTRAINT
	FK_ContractToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ContractToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ContractToMeter', N'CONSTRAINT', N'FK_ContractToMeter_SourceId'
GO
ALTER TABLE [Mapping].[ContractToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
