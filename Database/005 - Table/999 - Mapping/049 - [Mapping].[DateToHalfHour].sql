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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToHalfHour]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToHalfHour]
GO
CREATE TABLE [Mapping].[DateToHalfHour]
	(
	DateToHalfHourId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	HalfHourId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	PK_DateToHalfHour PRIMARY KEY CLUSTERED 
	(
	DateToHalfHourId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	DF_DateToHalfHour_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	DF_DateToHalfHour_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	DF_DateToHalfHour_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	FK_DateToHalfHour_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToHalfHour].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToHalfHour', N'CONSTRAINT', N'FK_DateToHalfHour_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	FK_DateToHalfHour_HalfHourId FOREIGN KEY
	(
	HalfHourId
	) REFERENCES [Information].[HalfHour]
	(
	HalfHourId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToHalfHour].HalfHourId to [Information].[HalfHour].HalfHourId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToHalfHour', N'CONSTRAINT', N'FK_DateToHalfHour_HalfHourId'
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	FK_DateToHalfHour_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToHalfHour].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToHalfHour', N'CONSTRAINT', N'FK_DateToHalfHour_DateId'
GO
ALTER TABLE [Mapping].[DateToHalfHour] ADD CONSTRAINT
	FK_DateToHalfHour_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToHalfHour].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToHalfHour', N'CONSTRAINT', N'FK_DateToHalfHour_SourceId'
GO
ALTER TABLE [Mapping].[DateToHalfHour] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
