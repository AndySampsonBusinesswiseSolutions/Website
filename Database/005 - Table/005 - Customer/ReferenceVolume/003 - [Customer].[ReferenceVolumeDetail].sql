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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[ReferenceVolumeDetail]') AND type in (N'U'))
DROP TABLE [Customer].[ReferenceVolumeDetail]
GO
CREATE TABLE [Customer].[ReferenceVolumeDetail]
	(
	ReferenceVolumeDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ReferenceVolumeId BIGINT NOT NULL,
	ReferenceVolumeAttributeId BIGINT NOT NULL,
	ReferenceVolumeDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	PK_ReferenceVolumeDetail PRIMARY KEY CLUSTERED 
	(
	ReferenceVolumeDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	DF_ReferenceVolumeDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	DF_ReferenceVolumeDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	DF_ReferenceVolumeDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	FK_ReferenceVolumeDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ReferenceVolumeDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ReferenceVolumeDetail', N'CONSTRAINT', N'FK_ReferenceVolumeDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	FK_ReferenceVolumeDetail_ReferenceVolumeId FOREIGN KEY
	(
	ReferenceVolumeId
	) REFERENCES [Customer].[ReferenceVolume]
	(
	ReferenceVolumeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ReferenceVolumeDetail].ReferenceVolumeId to [Customer].[ReferenceVolume].ReferenceVolumeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ReferenceVolumeDetail', N'CONSTRAINT', N'FK_ReferenceVolumeDetail_ReferenceVolumeId'
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	FK_ReferenceVolumeDetail_ReferenceVolumeAttributeId FOREIGN KEY
	(
	ReferenceVolumeAttributeId
	) REFERENCES [Customer].[ReferenceVolumeAttribute]
	(
	ReferenceVolumeAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ReferenceVolumeDetail].ReferenceVolumeAttributeId to [Customer].[ReferenceVolumeAttribute].ReferenceVolumeAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ReferenceVolumeDetail', N'CONSTRAINT', N'FK_ReferenceVolumeDetail_ReferenceVolumeAttributeId'
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] ADD CONSTRAINT
	FK_ReferenceVolumeDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[ReferenceVolumeDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'ReferenceVolumeDetail', N'CONSTRAINT', N'FK_ReferenceVolumeDetail_SourceId'
GO
ALTER TABLE [Customer].[ReferenceVolumeDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
