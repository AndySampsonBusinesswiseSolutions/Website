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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[RateUnit]') AND type in (N'U'))
DROP TABLE [Information].[RateUnit]
GO
CREATE TABLE [Information].[RateUnit]
	(
	RateUnitId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	RateUnitDescription VARCHAR(255) NOT NULL,
	)  ON [Information]
GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	PK_RateUnit PRIMARY KEY CLUSTERED 
	(
	RateUnitId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	DF_RateUnit_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	DF_RateUnit_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	DF_RateUnit_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	FK_RateUnit_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[RateUnit].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'RateUnit', N'CONSTRAINT', N'FK_RateUnit_CreatedByUserId'
GO
ALTER TABLE [Information].[RateUnit] ADD CONSTRAINT
	FK_RateUnit_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[RateUnit].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'RateUnit', N'CONSTRAINT', N'FK_RateUnit_SourceId'
GO
ALTER TABLE [Information].[RateUnit] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
