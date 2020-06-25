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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[AreaToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[AreaToMeter]
GO
CREATE TABLE [Mapping].[AreaToMeter]
	(
	AreaToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	AreaId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	PK_AreaToMeter PRIMARY KEY CLUSTERED 
	(
	AreaToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	DF_AreaToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	DF_AreaToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	DF_AreaToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	FK_AreaToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AreaToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AreaToMeter', N'CONSTRAINT', N'FK_AreaToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	FK_AreaToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AreaToMeter].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AreaToMeter', N'CONSTRAINT', N'FK_AreaToMeter_MeterId'
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	FK_AreaToMeter_AreaId FOREIGN KEY
	(
	AreaId
	) REFERENCES [Information].[Area]
	(
	AreaId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AreaToMeter].AreaId to [Information].[Area].AreaId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AreaToMeter', N'CONSTRAINT', N'FK_AreaToMeter_AreaId'
GO
ALTER TABLE [Mapping].[AreaToMeter] ADD CONSTRAINT
	FK_AreaToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[AreaToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'AreaToMeter', N'CONSTRAINT', N'FK_AreaToMeter_SourceId'
GO
ALTER TABLE [Mapping].[AreaToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
