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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[GranularityDetail]') AND type in (N'U'))
DROP TABLE [Information].[GranularityDetail]
GO
CREATE TABLE [Information].[GranularityDetail]
	(
	GranularityDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	GranularityId BIGINT NOT NULL,
	GranularityAttributeId BIGINT NOT NULL,
	GranularityDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	PK_GranularityDetail PRIMARY KEY CLUSTERED 
	(
	GranularityDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	DF_GranularityDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	DF_GranularityDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	DF_GranularityDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	FK_GranularityDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GranularityDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GranularityDetail', N'CONSTRAINT', N'FK_GranularityDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	FK_GranularityDetail_GranularityId FOREIGN KEY
	(
	GranularityId
	) REFERENCES [Information].[Granularity]
	(
	GranularityId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GranularityDetail].GranularityId to [Information].[Granularity].GranularityId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GranularityDetail', N'CONSTRAINT', N'FK_GranularityDetail_GranularityId'
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	FK_GranularityDetail_GranularityAttributeId FOREIGN KEY
	(
	GranularityAttributeId
	) REFERENCES [Information].[GranularityAttribute]
	(
	GranularityAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GranularityDetail].GranularityAttributeId to [Information].[GranularityAttribute].GranularityAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GranularityDetail', N'CONSTRAINT', N'FK_GranularityDetail_GranularityAttributeId'
GO
ALTER TABLE [Information].[GranularityDetail] ADD CONSTRAINT
	FK_GranularityDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GranularityDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GranularityDetail', N'CONSTRAINT', N'FK_GranularityDetail_SourceId'
GO
ALTER TABLE [Information].[GranularityDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
