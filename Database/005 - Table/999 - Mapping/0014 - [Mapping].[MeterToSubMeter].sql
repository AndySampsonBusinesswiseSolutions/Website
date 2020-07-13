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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MeterToSubMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[MeterToSubMeter]
GO
CREATE TABLE [Mapping].[MeterToSubMeter]
	(
	MeterToSubMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL,
	SubMeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	PK_MeterToSubMeter PRIMARY KEY CLUSTERED 
	(
	MeterToSubMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	DF_MeterToSubMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	DF_MeterToSubMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	DF_MeterToSubMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	FK_MeterToSubMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSubMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSubMeter', N'CONSTRAINT', N'FK_MeterToSubMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	FK_MeterToSubMeter_SubMeterId FOREIGN KEY
	(
	SubMeterId
	) REFERENCES [Customer].[SubMeter]
	(
	SubMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSubMeter].SubMeterId to [Meter].[SubMeter].SubMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSubMeter', N'CONSTRAINT', N'FK_MeterToSubMeter_SubMeterId'
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	FK_MeterToSubMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSubMeter].MeterId to [Meter].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSubMeter', N'CONSTRAINT', N'FK_MeterToSubMeter_MeterId'
GO
ALTER TABLE [Mapping].[MeterToSubMeter] ADD CONSTRAINT
	FK_MeterToSubMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSubMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSubMeter', N'CONSTRAINT', N'FK_MeterToSubMeter_SourceId'
GO
ALTER TABLE [Mapping].[MeterToSubMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
