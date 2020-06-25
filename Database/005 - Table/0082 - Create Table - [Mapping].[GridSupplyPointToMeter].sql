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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[GridSupplyPointToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[GridSupplyPointToMeter]
GO
CREATE TABLE [Mapping].[GridSupplyPointToMeter]
	(
	GridSupplyPointToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	GridSupplyPointId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	PK_GridSupplyPointToMeter PRIMARY KEY CLUSTERED 
	(
	GridSupplyPointToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	DF_GridSupplyPointToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	DF_GridSupplyPointToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	DF_GridSupplyPointToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	FK_GridSupplyPointToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GridSupplyPointToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GridSupplyPointToMeter', N'CONSTRAINT', N'FK_GridSupplyPointToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	FK_GridSupplyPointToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GridSupplyPointToMeter].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GridSupplyPointToMeter', N'CONSTRAINT', N'FK_GridSupplyPointToMeter_MeterId'
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	FK_GridSupplyPointToMeter_GridSupplyPointId FOREIGN KEY
	(
	GridSupplyPointId
	) REFERENCES [Information].[GridSupplyPoint]
	(
	GridSupplyPointId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GridSupplyPointToMeter].GridSupplyPointId to [Information].[GridSupplyPoint].GridSupplyPointId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GridSupplyPointToMeter', N'CONSTRAINT', N'FK_GridSupplyPointToMeter_GridSupplyPointId'
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] ADD CONSTRAINT
	FK_GridSupplyPointToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[GridSupplyPointToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'GridSupplyPointToMeter', N'CONSTRAINT', N'FK_GridSupplyPointToMeter_SourceId'
GO
ALTER TABLE [Mapping].[GridSupplyPointToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
