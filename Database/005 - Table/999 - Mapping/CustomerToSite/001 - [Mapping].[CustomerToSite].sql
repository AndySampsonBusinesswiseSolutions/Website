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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CustomerToSite]') AND type in (N'U'))
DROP TABLE [Mapping].[CustomerToSite]
GO
CREATE TABLE [Mapping].[CustomerToSite]
	(
	CustomerToSiteId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CustomerId BIGINT NOT NULL,
	SiteId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	PK_CustomerToSite PRIMARY KEY CLUSTERED 
	(
	CustomerToSiteId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	DF_CustomerToSite_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	DF_CustomerToSite_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	DF_CustomerToSite_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	FK_CustomerToSite_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToSite].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToSite', N'CONSTRAINT', N'FK_CustomerToSite_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	FK_CustomerToSite_SiteId FOREIGN KEY
	(
	SiteId
	) REFERENCES [Customer].[Site]
	(
	SiteId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToSite].SiteId to [Customer].[Site].SiteId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToSite', N'CONSTRAINT', N'FK_CustomerToSite_SiteId'
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	FK_CustomerToSite_CustomerId FOREIGN KEY
	(
	CustomerId
	) REFERENCES [Customer].[Customer]
	(
	CustomerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToSite].CustomerId to [Customer].[Customer].CustomerId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToSite', N'CONSTRAINT', N'FK_CustomerToSite_CustomerId'
GO
ALTER TABLE [Mapping].[CustomerToSite] ADD CONSTRAINT
	FK_CustomerToSite_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CustomerToSite].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CustomerToSite', N'CONSTRAINT', N'FK_CustomerToSite_SourceId'
GO
ALTER TABLE [Mapping].[CustomerToSite] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
