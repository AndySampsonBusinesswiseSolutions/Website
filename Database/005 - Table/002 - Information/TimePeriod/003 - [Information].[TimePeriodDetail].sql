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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[TimePeriodDetail]') AND type in (N'U'))
DROP TABLE [Information].[TimePeriodDetail]
GO
CREATE TABLE [Information].[TimePeriodDetail]
	(
	TimePeriodDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TimePeriodId BIGINT NOT NULL,
	TimePeriodAttributeId BIGINT NOT NULL,
	TimePeriodDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	PK_TimePeriodDetail PRIMARY KEY CLUSTERED 
	(
	TimePeriodDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	DF_TimePeriodDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	DF_TimePeriodDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	DF_TimePeriodDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	FK_TimePeriodDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[TimePeriodDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'TimePeriodDetail', N'CONSTRAINT', N'FK_TimePeriodDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	FK_TimePeriodDetail_TimePeriodId FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[TimePeriodDetail].TimePeriodId to [Information].[TimePeriod].TimePeriodId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'TimePeriodDetail', N'CONSTRAINT', N'FK_TimePeriodDetail_TimePeriodId'
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	FK_TimePeriodDetail_TimePeriodAttributeId FOREIGN KEY
	(
	TimePeriodAttributeId
	) REFERENCES [Information].[TimePeriodAttribute]
	(
	TimePeriodAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[TimePeriodDetail].TimePeriodAttributeId to [Information].[TimePeriodAttribute].TimePeriodAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'TimePeriodDetail', N'CONSTRAINT', N'FK_TimePeriodDetail_TimePeriodAttributeId'
GO
ALTER TABLE [Information].[TimePeriodDetail] ADD CONSTRAINT
	FK_TimePeriodDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[TimePeriodDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'TimePeriodDetail', N'CONSTRAINT', N'FK_TimePeriodDetail_SourceId'
GO
ALTER TABLE [Information].[TimePeriodDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
