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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToFiveMinute]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToFiveMinute]
GO
CREATE TABLE [Mapping].[DateToFiveMinute]
	(
	DateToFiveMinuteId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	FiveMinuteId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	PK_DateToFiveMinute PRIMARY KEY CLUSTERED 
	(
	DateToFiveMinuteId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	DF_DateToFiveMinute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	DF_DateToFiveMinute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	DF_DateToFiveMinute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	FK_DateToFiveMinute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToFiveMinute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToFiveMinute', N'CONSTRAINT', N'FK_DateToFiveMinute_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	FK_DateToFiveMinute_FiveMinuteId FOREIGN KEY
	(
	FiveMinuteId
	) REFERENCES [Information].[FiveMinute]
	(
	FiveMinuteId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToFiveMinute].FiveMinuteId to [Information].[FiveMinute].FiveMinuteId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToFiveMinute', N'CONSTRAINT', N'FK_DateToFiveMinute_FiveMinuteId'
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	FK_DateToFiveMinute_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToFiveMinute].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToFiveMinute', N'CONSTRAINT', N'FK_DateToFiveMinute_DateId'
GO
ALTER TABLE [Mapping].[DateToFiveMinute] ADD CONSTRAINT
	FK_DateToFiveMinute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToFiveMinute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToFiveMinute', N'CONSTRAINT', N'FK_DateToFiveMinute_SourceId'
GO
ALTER TABLE [Mapping].[DateToFiveMinute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
