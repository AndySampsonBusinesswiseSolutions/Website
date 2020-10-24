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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[ContractMeterDetail]') AND type in (N'U'))
DROP TABLE [Customer].[ContractMeterDetail]
GO
CREATE TABLE [Customer].[ContractMeterDetail]
	(
	ContractMeterDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractMeterId BIGINT NOT NULL,
	ContractMeterAttributeId BIGINT NOT NULL,
	ContractMeterDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	PK_ContractMeterDetail PRIMARY KEY CLUSTERED 
	(
	ContractMeterDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	DF_ContractMeterDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	DF_ContractMeterDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	DF_ContractMeterDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	FK_ContractMeterDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterDetail', N'CONSTRAINT', N'FK_ContractMeterDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	FK_ContractMeterDetail_ContractMeterAttributeId FOREIGN KEY
	(
	ContractMeterAttributeId
	) REFERENCES [Customer].[ContractMeterAttribute]
	(
	ContractMeterAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterDetail].ContractMeterAttributeId to [Customer].[ContractMeterAttribute].ContractMeterAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterDetail', N'CONSTRAINT', N'FK_ContractMeterDetail_ContractMeterAttributeId'
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	FK_ContractMeterDetail_ContractMeterId FOREIGN KEY
	(
	ContractMeterId
	) REFERENCES [Customer].[ContractMeter]
	(
	ContractMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterDetail].ContractMeterId to [Customer].[ContractMeter].ContractMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterDetail', N'CONSTRAINT', N'FK_ContractMeterDetail_ContractMeterId'
GO
ALTER TABLE [Customer].[ContractMeterDetail] ADD CONSTRAINT
	FK_ContractMeterDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractMeterDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractMeterDetail', N'CONSTRAINT', N'FK_ContractMeterDetail_SourceId'
GO
ALTER TABLE [Customer].[ContractMeterDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
