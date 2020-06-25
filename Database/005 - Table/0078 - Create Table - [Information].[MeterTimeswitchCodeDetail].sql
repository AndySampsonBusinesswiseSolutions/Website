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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[MeterTimeswitchCodeDetail]') AND type in (N'U'))
DROP TABLE [Information].[MeterTimeswitchCodeDetail]
GO
CREATE TABLE [Information].[MeterTimeswitchCodeDetail]
	(
	MeterTimeswitchCodeDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterTimeswitchCodeId BIGINT NOT NULL,
	MeterTimeswitchCodeAttributeId BIGINT NOT NULL,
	MeterTimeswitchCodeDetailDescription VARCHAR(200) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	PK_MeterTimeswitchCodeDetail PRIMARY KEY CLUSTERED 
	(
	MeterTimeswitchCodeDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	DF_MeterTimeswitchCodeDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	DF_MeterTimeswitchCodeDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	DF_MeterTimeswitchCodeDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	FK_MeterTimeswitchCodeDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterTimeswitchCodeDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterTimeswitchCodeDetail', N'CONSTRAINT', N'FK_MeterTimeswitchCodeDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	FK_MeterTimeswitchCodeDetail_MeterTimeswitchCodeId FOREIGN KEY
	(
	MeterTimeswitchCodeId
	) REFERENCES [Information].[MeterTimeswitchCode]
	(
	MeterTimeswitchCodeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterTimeswitchCodeDetail].MeterTimeswitchCodeId to [Information].[MeterTimeswitchCode].MeterTimeswitchCodeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterTimeswitchCodeDetail', N'CONSTRAINT', N'FK_MeterTimeswitchCodeDetail_MeterTimeswitchCodeId'
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	FK_MeterTimeswitchCodeDetail_MeterTimeswitchCodeAttributeId FOREIGN KEY
	(
	MeterTimeswitchCodeAttributeId
	) REFERENCES [Information].[MeterTimeswitchCodeAttribute]
	(
	MeterTimeswitchCodeAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterTimeswitchCodeDetail].MeterTimeswitchCodeAttributeId to [Information].[MeterTimeswitchCodeAttribute].MeterTimeswitchCodeAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterTimeswitchCodeDetail', N'CONSTRAINT', N'FK_MeterTimeswitchCodeDetail_MeterTimeswitchCodeAttributeId'
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] ADD CONSTRAINT
	FK_MeterTimeswitchCodeDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterTimeswitchCodeDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterTimeswitchCodeDetail', N'CONSTRAINT', N'FK_MeterTimeswitchCodeDetail_SourceId'
GO
ALTER TABLE [Information].[MeterTimeswitchCodeDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
