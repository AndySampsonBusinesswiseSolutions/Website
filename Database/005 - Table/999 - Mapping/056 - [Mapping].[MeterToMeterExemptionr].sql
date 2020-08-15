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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MeterToMeterExemption]') AND type in (N'U'))
DROP TABLE [Mapping].[MeterToMeterExemption]
GO
CREATE TABLE [Mapping].[MeterToMeterExemption]
	(
	MeterToMeterExemptionId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterExemptionId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	PK_MeterToMeterExemption PRIMARY KEY CLUSTERED 
	(
	MeterToMeterExemptionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	DF_MeterToMeterExemption_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	DF_MeterToMeterExemption_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	DF_MeterToMeterExemption_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	FK_MeterToMeterExemption_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterExemption].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterExemption', N'CONSTRAINT', N'FK_MeterToMeterExemption_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	FK_MeterToMeterExemption_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterExemption].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterExemption', N'CONSTRAINT', N'FK_MeterToMeterExemption_MeterId'
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	FK_MeterToMeterExemption_MeterExemptionId FOREIGN KEY
	(
	MeterExemptionId
	) REFERENCES [Information].[MeterExemption]
	(
	MeterExemptionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterExemption].MeterExemptionId to [Information].[MeterExemption].MeterExemptionId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterExemption', N'CONSTRAINT', N'FK_MeterToMeterExemption_MeterExemptionId'
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] ADD CONSTRAINT
	FK_MeterToMeterExemption_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterExemption].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterExemption', N'CONSTRAINT', N'FK_MeterToMeterExemption_SourceId'
GO
ALTER TABLE [Mapping].[MeterToMeterExemption] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
