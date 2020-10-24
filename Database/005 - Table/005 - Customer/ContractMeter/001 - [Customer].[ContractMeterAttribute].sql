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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[ContractMeterAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[ContractMeterAttribute]
GO
CREATE TABLE [Customer].[ContractMeterAttribute]
	(
	ContractMeterAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractMeterAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	PK_ContractMeterAttribute PRIMARY KEY CLUSTERED 
	(
	ContractMeterAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	DF_ContractMeterAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	DF_ContractMeterAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	DF_ContractMeterAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	FK_ContractMeterAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterAttribute', N'CONSTRAINT', N'FK_ContractMeterAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[ContractMeterAttribute] ADD CONSTRAINT
	FK_ContractMeterAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterAttribute', N'CONSTRAINT', N'FK_ContractMeterAttribute_SourceId'
GO
ALTER TABLE [Customer].[ContractMeterAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
