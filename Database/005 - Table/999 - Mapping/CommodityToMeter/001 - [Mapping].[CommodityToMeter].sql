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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToMeter]
GO
CREATE TABLE [Mapping].[CommodityToMeter]
	(
	CommodityToMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	PK_CommodityToMeter PRIMARY KEY CLUSTERED 
	(
	CommodityToMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	DF_CommodityToMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	DF_CommodityToMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	DF_CommodityToMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	FK_CommodityToMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToMeter', N'CONSTRAINT', N'FK_CommodityToMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	FK_CommodityToMeter_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToMeter].MeterId to [Commodity].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToMeter', N'CONSTRAINT', N'FK_CommodityToMeter_MeterId'
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	FK_CommodityToMeter_CommodityId FOREIGN KEY
	(
	CommodityId
	) REFERENCES [Information].[Commodity]
	(
	CommodityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToMeter].CommodityId to [Information].[Commodity].CommodityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToMeter', N'CONSTRAINT', N'FK_CommodityToMeter_CommodityId'
GO
ALTER TABLE [Mapping].[CommodityToMeter] ADD CONSTRAINT
	FK_CommodityToMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToMeter', N'CONSTRAINT', N'FK_CommodityToMeter_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
