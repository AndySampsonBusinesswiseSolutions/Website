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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[SubAreaToSubMeter]') AND type in (N'U'))
DROP TABLE [Mapping].[SubAreaToSubMeter]
GO
CREATE TABLE [Mapping].[SubAreaToSubMeter]
	(
	SubAreaToSubMeterId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	SubAreaId BIGINT NOT NULL,
	SubMeterId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	PK_SubAreaToSubMeter PRIMARY KEY CLUSTERED 
	(
	SubAreaToSubMeterId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	DF_SubAreaToSubMeter_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	DF_SubAreaToSubMeter_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	DF_SubAreaToSubMeter_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	FK_SubAreaToSubMeter_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[SubAreaToSubMeter].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'SubAreaToSubMeter', N'CONSTRAINT', N'FK_SubAreaToSubMeter_CreatedByUserId'
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	FK_SubAreaToSubMeter_SubMeterId FOREIGN KEY
	(
	SubMeterId
	) REFERENCES [Customer].[SubMeter]
	(
	SubMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[SubAreaToSubMeter].SubMeterId to [Customer].[SubMeter].SubMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'SubAreaToSubMeter', N'CONSTRAINT', N'FK_SubAreaToSubMeter_SubMeterId'
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	FK_SubAreaToSubMeter_SubAreaId FOREIGN KEY
	(
	SubAreaId
	) REFERENCES [Information].[SubArea]
	(
	SubAreaId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[SubAreaToSubMeter].SubAreaId to [Information].[SubArea].SubAreaId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'SubAreaToSubMeter', N'CONSTRAINT', N'FK_SubAreaToSubMeter_SubAreaId'
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] ADD CONSTRAINT
	FK_SubAreaToSubMeter_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[SubAreaToSubMeter].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'SubAreaToSubMeter', N'CONSTRAINT', N'FK_SubAreaToSubMeter_SourceId'
GO
ALTER TABLE [Mapping].[SubAreaToSubMeter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
