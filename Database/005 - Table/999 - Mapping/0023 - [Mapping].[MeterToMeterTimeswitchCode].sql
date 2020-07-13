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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MeterToMeterTimeswitchCode]') AND type in (N'U'))
DROP TABLE [Mapping].[MeterToMeterTimeswitchCode]
GO
CREATE TABLE [Mapping].[MeterToMeterTimeswitchCode]
	(
	MeterToMeterTimeswitchCodeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL,
	MeterTimeswitchCodeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	PK_MeterToMeterTimeswitchCode PRIMARY KEY CLUSTERED 
	(
	MeterToMeterTimeswitchCodeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	DF_MeterToMeterTimeswitchCode_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	DF_MeterToMeterTimeswitchCode_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	DF_MeterToMeterTimeswitchCode_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	FK_MeterToMeterTimeswitchCode_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterTimeswitchCode].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterTimeswitchCode', N'CONSTRAINT', N'FK_MeterToMeterTimeswitchCode_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	FK_MeterToMeterTimeswitchCode_MeterTimeswitchCodeId FOREIGN KEY
	(
	MeterTimeswitchCodeId
	) REFERENCES [Information].[MeterTimeswitchCode]
	(
	MeterTimeswitchCodeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterTimeswitchCode].MeterTimeswitchCodeId to [Information].[MeterTimeswitchCode].MeterTimeswitchCodeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterTimeswitchCode', N'CONSTRAINT', N'FK_MeterToMeterTimeswitchCode_MeterTimeswitchCodeId'
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	FK_MeterToMeterTimeswitchCode_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterTimeswitchCode].MeterId to [Information].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterTimeswitchCode', N'CONSTRAINT', N'FK_MeterToMeterTimeswitchCode_MeterId'
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] ADD CONSTRAINT
	FK_MeterToMeterTimeswitchCode_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToMeterTimeswitchCode].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToMeterTimeswitchCode', N'CONSTRAINT', N'FK_MeterToMeterTimeswitchCode_SourceId'
GO
ALTER TABLE [Mapping].[MeterToMeterTimeswitchCode] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
