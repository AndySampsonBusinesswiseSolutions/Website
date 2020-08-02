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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToPeriodToDate]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToPeriodToDate]
GO
CREATE TABLE [Mapping].[CommodityToPeriodToDate]
	(
	CommodityToPeriodToDateId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityToPeriodId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	PK_CommodityToPeriodToDate PRIMARY KEY CLUSTERED 
	(
	CommodityToPeriodToDateId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	DF_CommodityToPeriodToDate_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	DF_CommodityToPeriodToDate_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	DF_CommodityToPeriodToDate_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	FK_CommodityToPeriodToDate_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToPeriodToDate].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToPeriodToDate', N'CONSTRAINT', N'FK_CommodityToPeriodToDate_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	FK_CommodityToPeriodToDate_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToPeriodToDate].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToPeriodToDate', N'CONSTRAINT', N'FK_CommodityToPeriodToDate_DateId'
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	FK_CommodityToPeriodToDate_CommodityToPeriodId FOREIGN KEY
	(
	CommodityToPeriodId
	) REFERENCES [Mapping].[CommodityToPeriod]
	(
	CommodityToPeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToPeriodToDate].CommodityToPeriodId to [Mapping].[CommodityToPeriod].CommodityToPeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToPeriodToDate', N'CONSTRAINT', N'FK_CommodityToPeriodToDate_CommodityToPeriodId'
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] ADD CONSTRAINT
	FK_CommodityToPeriodToDate_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToPeriodToDate].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToPeriodToDate', N'CONSTRAINT', N'FK_CommodityToPeriodToDate_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToPeriodToDate] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
