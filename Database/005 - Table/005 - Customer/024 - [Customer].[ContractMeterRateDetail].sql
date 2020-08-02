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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[ContractMeterRateDetail]') AND type in (N'U'))
DROP TABLE [Customer].[ContractMeterRateDetail]
GO
CREATE TABLE [Customer].[ContractMeterRateDetail]
	(
	ContractMeterRateDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractMeterRateId BIGINT NOT NULL,
	ContractMeterRateAttributeId BIGINT NOT NULL,
	ContractMeterRateDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	PK_ContractMeterRateDetail PRIMARY KEY CLUSTERED 
	(
	ContractMeterRateDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	DF_ContractMeterRateDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	DF_ContractMeterRateDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	DF_ContractMeterRateDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	FK_ContractMeterRateDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterRateDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterRateDetail', N'CONSTRAINT', N'FK_ContractMeterRateDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	FK_ContractMeterRateDetail_ContractMeterRateAttributeId FOREIGN KEY
	(
	ContractMeterRateAttributeId
	) REFERENCES [Customer].[ContractMeterRateAttribute]
	(
	ContractMeterRateAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterRateDetail].ContractMeterRateAttributeId to [Customer].[ContractMeterRateAttribute].ContractMeterRateAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterRateDetail', N'CONSTRAINT', N'FK_ContractMeterRateDetail_ContractMeterRateAttributeId'
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	FK_ContractMeterRateDetail_ContractMeterRateId FOREIGN KEY
	(
	ContractMeterRateId
	) REFERENCES [Customer].[ContractMeterRate]
	(
	ContractMeterRateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterRateDetail].ContractMeterRateId to [Customer].[ContractMeterRate].ContractMeterRateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterRateDetail', N'CONSTRAINT', N'FK_ContractMeterRateDetail_ContractMeterRateId'
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] ADD CONSTRAINT
	FK_ContractMeterRateDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterRateDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterRateDetail', N'CONSTRAINT', N'FK_ContractMeterRateDetail_SourceId'
GO
ALTER TABLE [Customer].[ContractMeterRateDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
