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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToForecastGroup]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToForecastGroup]
GO
CREATE TABLE [Mapping].[DateToForecastGroup]
	(
	DateToForecastGroupId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	ForecastGroupId BIGINT NOT NULL,
	Priority INT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	PK_DateToForecastGroup PRIMARY KEY CLUSTERED 
	(
	DateToForecastGroupId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	DF_DateToForecastGroup_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	DF_DateToForecastGroup_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	DF_DateToForecastGroup_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	FK_DateToForecastGroup_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToForecastGroup].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToForecastGroup', N'CONSTRAINT', N'FK_DateToForecastGroup_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	FK_DateToForecastGroup_ForecastGroupId FOREIGN KEY
	(
	ForecastGroupId
	) REFERENCES [DemandForecast].[ForecastGroup]
	(
	ForecastGroupId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToForecastGroup].ForecastGroupId to [DemandForecast].[ForecastGroup].ForecastGroupId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToForecastGroup', N'CONSTRAINT', N'FK_DateToForecastGroup_ForecastGroupId'
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	FK_DateToForecastGroup_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToForecastGroup].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToForecastGroup', N'CONSTRAINT', N'FK_DateToForecastGroup_DateId'
GO
ALTER TABLE [Mapping].[DateToForecastGroup] ADD CONSTRAINT
	FK_DateToForecastGroup_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToForecastGroup].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToForecastGroup', N'CONSTRAINT', N'FK_DateToForecastGroup_SourceId'
GO
ALTER TABLE [Mapping].[DateToForecastGroup] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
