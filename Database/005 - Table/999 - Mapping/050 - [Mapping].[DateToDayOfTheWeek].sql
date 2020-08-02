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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToDayOfTheWeek]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToDayOfTheWeek]
GO
CREATE TABLE [Mapping].[DateToDayOfTheWeek]
	(
	DateToDayOfTheWeekId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	DayOfTheWeekId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	PK_DateToDayOfTheWeek PRIMARY KEY CLUSTERED 
	(
	DateToDayOfTheWeekId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	DF_DateToDayOfTheWeek_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	DF_DateToDayOfTheWeek_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	DF_DateToDayOfTheWeek_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	FK_DateToDayOfTheWeek_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToDayOfTheWeek].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToDayOfTheWeek', N'CONSTRAINT', N'FK_DateToDayOfTheWeek_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	FK_DateToDayOfTheWeek_DayOfTheWeekId FOREIGN KEY
	(
	DayOfTheWeekId
	) REFERENCES [Information].[DayOfTheWeek]
	(
	DayOfTheWeekId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToDayOfTheWeek].DayOfTheWeekId to [Information].[DayOfTheWeek].DayOfTheWeekId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToDayOfTheWeek', N'CONSTRAINT', N'FK_DateToDayOfTheWeek_DayOfTheWeekId'
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	FK_DateToDayOfTheWeek_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToDayOfTheWeek].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToDayOfTheWeek', N'CONSTRAINT', N'FK_DateToDayOfTheWeek_DateId'
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] ADD CONSTRAINT
	FK_DateToDayOfTheWeek_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToDayOfTheWeek].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToDayOfTheWeek', N'CONSTRAINT', N'FK_DateToDayOfTheWeek_SourceId'
GO
ALTER TABLE [Mapping].[DateToDayOfTheWeek] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
