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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToMonth]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToMonth]
GO
CREATE TABLE [Mapping].[DateToMonth]
	(
	DateToMonthId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	MonthId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	PK_DateToMonth PRIMARY KEY CLUSTERED 
	(
	DateToMonthId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	DF_DateToMonth_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	DF_DateToMonth_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	DF_DateToMonth_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	FK_DateToMonth_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToMonth].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToMonth', N'CONSTRAINT', N'FK_DateToMonth_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	FK_DateToMonth_MonthId FOREIGN KEY
	(
	MonthId
	) REFERENCES [Information].[Month]
	(
	MonthId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToMonth].MonthId to [Information].[Month].MonthId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToMonth', N'CONSTRAINT', N'FK_DateToMonth_MonthId'
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	FK_DateToMonth_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToMonth].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToMonth', N'CONSTRAINT', N'FK_DateToMonth_DateId'
GO
ALTER TABLE [Mapping].[DateToMonth] ADD CONSTRAINT
	FK_DateToMonth_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToMonth].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToMonth', N'CONSTRAINT', N'FK_DateToMonth_SourceId'
GO
ALTER TABLE [Mapping].[DateToMonth] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
