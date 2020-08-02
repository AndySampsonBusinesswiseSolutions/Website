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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[LineLossFactorClassToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[LineLossFactorClassToMeter]
GO
CREATE TABLE [Mapping].[LineLossFactorClassToMeter]
	(
	LineLossFactorClassToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	LineLossFactorClassId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	PK_LineLossFactorClassToMeter PRIMARY KEY CLUSTERED 
	(
	LineLossFactorClassToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	DF_LineLossFactorClassToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	DF_LineLossFactorClassToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	DF_LineLossFactorClassToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	FK_LineLossFactorClassToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LineLossFactorClassToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LineLossFactorClassToMeter', N'CONSTRAINT', N'FK_LineLossFactorClassToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	FK_LineLossFactorClassToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LineLossFactorClassToMeter].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LineLossFactorClassToMeter', N'CONSTRAINT', N'FK_LineLossFactorClassToMeter_MeterId'
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	FK_LineLossFactorClassToMeter_LineLossFactorClassId FOREIGN KEY
	(
	LineLossFactorClassId
	) REFERENCES [Information].[LineLossFactorClass]
	(
	LineLossFactorClassId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LineLossFactorClassToMeter].LineLossFactorClassId to [Information].[LineLossFactorClass].LineLossFactorClassId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LineLossFactorClassToMeter', N'CONSTRAINT', N'FK_LineLossFactorClassToMeter_LineLossFactorClassId'
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] ADD CONSTRAINT
	FK_LineLossFactorClassToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[LineLossFactorClassToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'LineLossFactorClassToMeter', N'CONSTRAINT', N'FK_LineLossFactorClassToMeter_SourceId'
GO
ALTER TABLE [Mapping].[LineLossFactorClassToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
