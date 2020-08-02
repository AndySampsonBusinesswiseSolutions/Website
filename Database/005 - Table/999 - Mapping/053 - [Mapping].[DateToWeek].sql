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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToWeek]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToWeek]
GO
CREATE TABLE [Mapping].[DateToWeek]
	(
	DateToWeekId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	WeekId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	PK_DateToWeek PRIMARY KEY CLUSTERED 
	(
	DateToWeekId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	DF_DateToWeek_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	DF_DateToWeek_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	DF_DateToWeek_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	FK_DateToWeek_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToWeek].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToWeek', N'CONSTRAINT', N'FK_DateToWeek_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	FK_DateToWeek_WeekId FOREIGN KEY
	(
	WeekId
	) REFERENCES [Information].[Week]
	(
	WeekId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToWeek].WeekId to [Information].[Week].WeekId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToWeek', N'CONSTRAINT', N'FK_DateToWeek_WeekId'
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	FK_DateToWeek_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToWeek].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToWeek', N'CONSTRAINT', N'FK_DateToWeek_DateId'
GO
ALTER TABLE [Mapping].[DateToWeek] ADD CONSTRAINT
	FK_DateToWeek_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToWeek].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToWeek', N'CONSTRAINT', N'FK_DateToWeek_SourceId'
GO
ALTER TABLE [Mapping].[DateToWeek] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
