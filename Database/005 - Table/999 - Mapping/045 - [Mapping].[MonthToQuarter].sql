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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MonthToQuarter]') AND type in (N'U'))
DROP TABLE [Mapping].[MonthToQuarter]
GO
CREATE TABLE [Mapping].[MonthToQuarter]
	(
	MonthToQuarterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MonthId BIGINT NOT NULL,
	QuarterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	PK_MonthToQuarter PRIMARY KEY CLUSTERED 
	(
	MonthToQuarterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	DF_MonthToQuarter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	DF_MonthToQuarter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	DF_MonthToQuarter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	FK_MonthToQuarter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MonthToQuarter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MonthToQuarter', N'CONSTRAINT', N'FK_MonthToQuarter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	FK_MonthToQuarter_QuarterId FOREIGN KEY
	(
	QuarterId
	) REFERENCES [Information].[Quarter]
	(
	QuarterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MonthToQuarter].QuarterId to [Information].[Quarter].QuarterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MonthToQuarter', N'CONSTRAINT', N'FK_MonthToQuarter_QuarterId'
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	FK_MonthToQuarter_MonthId FOREIGN KEY
	(
	MonthId
	) REFERENCES [Information].[Month]
	(
	MonthId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MonthToQuarter].MonthId to [Information].[Month].MonthId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MonthToQuarter', N'CONSTRAINT', N'FK_MonthToQuarter_MonthId'
GO
ALTER TABLE [Mapping].[MonthToQuarter] ADD CONSTRAINT
	FK_MonthToQuarter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MonthToQuarter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MonthToQuarter', N'CONSTRAINT', N'FK_MonthToQuarter_SourceId'
GO
ALTER TABLE [Mapping].[MonthToQuarter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
