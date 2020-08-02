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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[ContractDetail]') AND type in (N'U'))
DROP TABLE [Customer].[ContractDetail]
GO
CREATE TABLE [Customer].[ContractDetail]
	(
	ContractDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ContractId BIGINT NOT NULL,
	ContractAttributeId BIGINT NOT NULL,
	ContractDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	PK_ContractDetail PRIMARY KEY CLUSTERED 
	(
	ContractDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	DF_ContractDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	DF_ContractDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	DF_ContractDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	FK_ContractDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractDetail', N'CONSTRAINT', N'FK_ContractDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	FK_ContractDetail_ContractId FOREIGN KEY
	(
	ContractId
	) REFERENCES [Customer].[Contract]
	(
	ContractId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractDetail].ContractId to [Customer].[Contract].ContractId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractDetail', N'CONSTRAINT', N'FK_ContractDetail_ContractId'
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	FK_ContractDetail_ContractAttributeId FOREIGN KEY
	(
	ContractAttributeId
	) REFERENCES [Customer].[ContractAttribute]
	(
	ContractAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractDetail].ContractAttributeId to [Customer].[ContractAttribute].ContractAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractDetail', N'CONSTRAINT', N'FK_ContractDetail_ContractAttributeId'
GO
ALTER TABLE [Customer].[ContractDetail] ADD CONSTRAINT
	FK_ContractDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ContractDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ContractDetail', N'CONSTRAINT', N'FK_ContractDetail_SourceId'
GO
ALTER TABLE [Customer].[ContractDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
