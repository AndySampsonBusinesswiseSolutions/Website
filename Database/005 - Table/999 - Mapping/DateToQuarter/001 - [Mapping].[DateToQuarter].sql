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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[DateToQuarter]') AND type in (N'U'))
DROP TABLE [Mapping].[DateToQuarter]
GO
CREATE TABLE [Mapping].[DateToQuarter]
	(
	DateToQuarterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	QuarterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	PK_DateToQuarter PRIMARY KEY CLUSTERED 
	(
	DateToQuarterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	DF_DateToQuarter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	DF_DateToQuarter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	DF_DateToQuarter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	FK_DateToQuarter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToQuarter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToQuarter', N'CONSTRAINT', N'FK_DateToQuarter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	FK_DateToQuarter_QuarterId FOREIGN KEY
	(
	QuarterId
	) REFERENCES [Information].[Quarter]
	(
	QuarterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToQuarter].QuarterId to [Information].[Quarter].QuarterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToQuarter', N'CONSTRAINT', N'FK_DateToQuarter_QuarterId'
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	FK_DateToQuarter_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToQuarter].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToQuarter', N'CONSTRAINT', N'FK_DateToQuarter_DateId'
GO
ALTER TABLE [Mapping].[DateToQuarter] ADD CONSTRAINT
	FK_DateToQuarter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[DateToQuarter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'DateToQuarter', N'CONSTRAINT', N'FK_DateToQuarter_SourceId'
GO
ALTER TABLE [Mapping].[DateToQuarter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
