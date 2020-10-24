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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToTimePeriodToDateToProfileValue]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue]
GO
CREATE TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue]
	(
	CommodityToTimePeriodToDateToProfileValueId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityToTimePeriodToDateId BIGINT NOT NULL,
	ProfileValueId BIGINT NOT NULL,
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	PK_CommodityToTimePeriodToDateToProfileValue PRIMARY KEY CLUSTERED 
	(
	CommodityToTimePeriodToDateToProfileValueId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDateToProfileValue_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDateToProfileValue_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDateToProfileValue_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDateToProfileValue_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDateToProfileValue].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDateToProfileValue', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDateToProfileValue_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDateToProfileValue_ProfileValueId FOREIGN KEY
	(
	ProfileValueId
	) REFERENCES [DemandForecast].[ProfileValue]
	(
	ProfileValueId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDateToProfileValue].ProfileValueId to [DemandForecast].[ProfileValue].ProfileValueId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDateToProfileValue', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDateToProfileValue_ProfileValueId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDateToProfileValue_CommodityToTimePeriodToDateId FOREIGN KEY
	(
	CommodityToTimePeriodToDateId
	) REFERENCES [Mapping].[CommodityToTimePeriodToDate]
	(
	CommodityToTimePeriodToDateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDateToProfileValue].CommodityToTimePeriodToDateId to [Mapping].[CommodityToTimePeriodToDate].CommodityToTimePeriodToDateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDateToProfileValue', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDateToProfileValue_CommodityToTimePeriodToDateId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDateToProfileValue_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDateToProfileValue].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDateToProfileValue', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDateToProfileValue_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDateToProfileValue] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
