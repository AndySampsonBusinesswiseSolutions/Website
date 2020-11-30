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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[MeterToSite]') AND type in (N'U'))
DROP TABLE [Mapping].[MeterToSite]
GO
CREATE TABLE [Mapping].[MeterToSite]
	(
	MeterToSiteId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL,
	SiteId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	PK_MeterToSite PRIMARY KEY CLUSTERED 
	(
	MeterToSiteId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	DF_MeterToSite_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	DF_MeterToSite_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	DF_MeterToSite_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	FK_MeterToSite_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSite].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSite', N'CONSTRAINT', N'FK_MeterToSite_CreatedByUserId'
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	FK_MeterToSite_SiteId FOREIGN KEY
	(
	SiteId
	) REFERENCES [Customer].[Site]
	(
	SiteId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSite].SiteId to [Meter].[Site].SiteId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSite', N'CONSTRAINT', N'FK_MeterToSite_SiteId'
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	FK_MeterToSite_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSite].MeterId to [Meter].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSite', N'CONSTRAINT', N'FK_MeterToSite_MeterId'
GO
ALTER TABLE [Mapping].[MeterToSite] ADD CONSTRAINT
	FK_MeterToSite_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[MeterToSite].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'MeterToSite', N'CONSTRAINT', N'FK_MeterToSite_SourceId'
GO
ALTER TABLE [Mapping].[MeterToSite] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
