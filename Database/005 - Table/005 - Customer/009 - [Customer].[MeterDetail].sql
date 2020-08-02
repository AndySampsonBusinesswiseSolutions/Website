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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[MeterDetail]') AND type in (N'U'))
DROP TABLE [Customer].[MeterDetail]
GO
CREATE TABLE [Customer].[MeterDetail]
	(
	MeterDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterId BIGINT NOT NULL,
	MeterAttributeId BIGINT NOT NULL,
	MeterDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	PK_MeterDetail PRIMARY KEY CLUSTERED 
	(
	MeterDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	DF_MeterDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	DF_MeterDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	DF_MeterDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	FK_MeterDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterDetail', N'CONSTRAINT', N'FK_MeterDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	FK_MeterDetail_MeterId FOREIGN KEY
	(
	MeterId
	) REFERENCES [Customer].[Meter]
	(
	MeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterDetail].MeterId to [Customer].[Meter].MeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterDetail', N'CONSTRAINT', N'FK_MeterDetail_MeterId'
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	FK_MeterDetail_MeterAttributeId FOREIGN KEY
	(
	MeterAttributeId
	) REFERENCES [Customer].[MeterAttribute]
	(
	MeterAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterDetail].MeterAttributeId to [Customer].[MeterAttribute].MeterAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterDetail', N'CONSTRAINT', N'FK_MeterDetail_MeterAttributeId'
GO
ALTER TABLE [Customer].[MeterDetail] ADD CONSTRAINT
	FK_MeterDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterDetail', N'CONSTRAINT', N'FK_MeterDetail_SourceId'
GO
ALTER TABLE [Customer].[MeterDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
