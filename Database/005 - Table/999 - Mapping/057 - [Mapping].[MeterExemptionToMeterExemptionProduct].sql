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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MeterExemptionToMeterExemptionProduct]') AND type in (N'U'))
DROP TABLE [Mapping].[MeterExemptionToMeterExemptionProduct]
GO
CREATE TABLE [Mapping].[MeterExemptionToMeterExemptionProduct]
	(
	MeterExemptionToMeterExemptionProductId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterExemptionId BIGINT NOT NULL,
	MeterExemptionProductId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	PK_MeterExemptionToMeterExemptionProduct PRIMARY KEY CLUSTERED 
	(
	MeterExemptionToMeterExemptionProductId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	DF_MeterExemptionToMeterExemptionProduct_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	DF_MeterExemptionToMeterExemptionProduct_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	DF_MeterExemptionToMeterExemptionProduct_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	FK_MeterExemptionToMeterExemptionProduct_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterExemptionToMeterExemptionProduct].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterExemptionToMeterExemptionProduct', N'CONSTRAINT', N'FK_MeterExemptionToMeterExemptionProduct_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	FK_MeterExemptionToMeterExemptionProduct_MeterExemptionProductId FOREIGN KEY
	(
	MeterExemptionProductId
	) REFERENCES [Information].[MeterExemption]
	(
	MeterExemptionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterExemptionToMeterExemptionProduct].MeterExemptionProductId to [Information].[MeterExemption].MeterExemptionId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterExemptionToMeterExemptionProduct', N'CONSTRAINT', N'FK_MeterExemptionToMeterExemptionProduct_MeterExemptionProductId'
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	FK_MeterExemptionToMeterExemptionProduct_MeterExemptionId FOREIGN KEY
	(
	MeterExemptionId
	) REFERENCES [Customer].[MeterExemption]
	(
	MeterExemptionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterExemptionToMeterExemptionProduct].MeterExemptionId to [Customer].[MeterExemption].MeterExemptionId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterExemptionToMeterExemptionProduct', N'CONSTRAINT', N'FK_MeterExemptionToMeterExemptionProduct_MeterExemptionId'
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] ADD CONSTRAINT
	FK_MeterExemptionToMeterExemptionProduct_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterExemptionToMeterExemptionProduct].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterExemptionToMeterExemptionProduct', N'CONSTRAINT', N'FK_MeterExemptionToMeterExemptionProduct_SourceId'
GO
ALTER TABLE [Mapping].[MeterExemptionToMeterExemptionProduct] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
