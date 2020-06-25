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
USE [EMaaS]
GO
CREATE TABLE [DemandForecast].[ProfileDetail]
	(
	ProfileDetailId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProfileId BIGINT NOT NULL,
	ProfileAttributeId BIGINT NOT NULL,
	ProfileDetailDescription VARCHAR(10) NOT NULL
	)  ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	DF_ProfileDetail_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	DF_ProfileDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	DF_ProfileDetail_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	PK_ProfileDetail PRIMARY KEY CLUSTERED 
	(
	ProfileDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [DemandForecast]
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	FK_ProfileDetail_ProfileId FOREIGN KEY
	(
	ProfileId
	) REFERENCES [DemandForecast].[Profile]
	(
	ProfileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileDetail].SourceId to [DemandForecast].[Profile].ProfileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileDetail', N'CONSTRAINT', N'FK_ProfileDetail_ProfileId'
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	FK_ProfileDetail_ProfileAttributeId FOREIGN KEY
	(
	ProfileAttributeId
	) REFERENCES [DemandForecast].[ProfileAttribute]
	(
	ProfileAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileDetail].ProfileAttributeId to [DemandForecast].[ProfileAttribute].ProfileAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileDetail', N'CONSTRAINT', N'FK_ProfileDetail_ProfileAttributeId'
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	FK_ProfileDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileDetail', N'CONSTRAINT', N'FK_ProfileDetail_CreatedByUserId'
GO
ALTER TABLE [DemandForecast].[ProfileDetail] ADD CONSTRAINT
	FK_ProfileDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [DemandForecast].[ProfileDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'DemandForecast', N'TABLE', N'ProfileDetail', N'CONSTRAINT', N'FK_ProfileDetail_SourceId'
GO
ALTER TABLE [DemandForecast].[ProfileDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
