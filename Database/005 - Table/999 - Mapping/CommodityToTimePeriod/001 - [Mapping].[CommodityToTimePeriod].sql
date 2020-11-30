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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToTimePeriod]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToTimePeriod]
GO
CREATE TABLE [Mapping].[CommodityToTimePeriod]
	(
	CommodityToTimePeriodId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	PK_CommodityToTimePeriod PRIMARY KEY CLUSTERED 
	(
	CommodityToTimePeriodId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	DF_CommodityToTimePeriod_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	DF_CommodityToTimePeriod_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	DF_CommodityToTimePeriod_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	FK_CommodityToTimePeriod_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriod].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriod', N'CONSTRAINT', N'FK_CommodityToTimePeriod_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	FK_CommodityToTimePeriod_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriod].TimePeriodId to [Customer].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriod', N'CONSTRAINT', N'FK_CommodityToTimePeriod_TimePeriodId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	FK_CommodityToTimePeriod_CommodityId FOREIGN KEY
	(
	CommodityId
	) REFERENCES [Information].[Commodity]
	(
	CommodityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriod].CommodityId to [Information].[Commodity].CommodityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriod', N'CONSTRAINT', N'FK_CommodityToTimePeriod_CommodityId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] ADD CONSTRAINT
	FK_CommodityToTimePeriod_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriod].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriod', N'CONSTRAINT', N'FK_CommodityToTimePeriod_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriod] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
