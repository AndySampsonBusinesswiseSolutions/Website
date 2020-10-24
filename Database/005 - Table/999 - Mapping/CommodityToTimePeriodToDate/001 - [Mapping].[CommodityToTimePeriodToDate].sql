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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToTimePeriodToDate]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToTimePeriodToDate]
GO
CREATE TABLE [Mapping].[CommodityToTimePeriodToDate]
	(
	CommodityToTimePeriodToDateId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityToTimePeriodId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	PK_CommodityToTimePeriodToDate PRIMARY KEY CLUSTERED 
	(
	CommodityToTimePeriodToDateId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDate_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDate_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	DF_CommodityToTimePeriodToDate_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDate_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDate].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDate', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDate_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDate_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDate].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDate', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDate_DateId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDate_CommodityToTimePeriodId FOREIGN KEY
	(
	CommodityToTimePeriodId
	) REFERENCES [Mapping].[CommodityToTimePeriod]
	(
	CommodityToTimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDate].CommodityToTimePeriodId to [Mapping].[CommodityToTimePeriod].CommodityToTimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDate', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDate_CommodityToTimePeriodId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] ADD CONSTRAINT
	FK_CommodityToTimePeriodToDate_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToTimePeriodToDate].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToTimePeriodToDate', N'CONSTRAINT', N'FK_CommodityToTimePeriodToDate_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToTimePeriodToDate] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
