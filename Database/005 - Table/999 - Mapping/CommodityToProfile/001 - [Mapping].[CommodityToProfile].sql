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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[CommodityToProfile]') AND type in (N'U'))
DROP TABLE [Mapping].[CommodityToProfile]
GO
CREATE TABLE [Mapping].[CommodityToProfile]
	(
	CommodityToProfileId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CommodityId BIGINT NOT NULL,
	ProfileId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	PK_CommodityToProfile PRIMARY KEY CLUSTERED 
	(
	CommodityToProfileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	DF_CommodityToProfile_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	DF_CommodityToProfile_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	DF_CommodityToProfile_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	FK_CommodityToProfile_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToProfile].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToProfile', N'CONSTRAINT', N'FK_CommodityToProfile_CreatedByUserId'
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	FK_CommodityToProfile_ProfileId FOREIGN KEY
	(
	ProfileId
	) REFERENCES [DemandForecast].[Profile]
	(
	ProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToProfile].ProfileId to [DemandForecast].[Profile].ProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToProfile', N'CONSTRAINT', N'FK_CommodityToProfile_ProfileId'
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	FK_CommodityToProfile_CommodityId FOREIGN KEY
	(
	CommodityId
	) REFERENCES [Information].[Commodity]
	(
	CommodityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToProfile].CommodityId to [Information].[Commodity].CommodityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToProfile', N'CONSTRAINT', N'FK_CommodityToProfile_CommodityId'
GO
ALTER TABLE [Mapping].[CommodityToProfile] ADD CONSTRAINT
	FK_CommodityToProfile_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[CommodityToProfile].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'CommodityToProfile', N'CONSTRAINT', N'FK_CommodityToProfile_SourceId'
GO
ALTER TABLE [Mapping].[CommodityToProfile] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
