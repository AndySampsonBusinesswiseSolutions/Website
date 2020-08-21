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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[MeterExemptionDetail]') AND type in (N'U'))
DROP TABLE [Information].[MeterExemptionDetail]
GO
CREATE TABLE [Information].[MeterExemptionDetail]
	(
	MeterExemptionDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterExemptionId BIGINT NOT NULL,
	MeterExemptionAttributeId BIGINT NOT NULL,
	MeterExemptionDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	PK_MeterExemptionDetail PRIMARY KEY CLUSTERED 
	(
	MeterExemptionDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	DF_MeterExemptionDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	DF_MeterExemptionDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	DF_MeterExemptionDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	FK_MeterExemptionDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterExemptionDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterExemptionDetail', N'CONSTRAINT', N'FK_MeterExemptionDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	FK_MeterExemptionDetail_MeterExemptionId FOREIGN KEY
	(
	MeterExemptionId
	) REFERENCES [Information].[MeterExemption]
	(
	MeterExemptionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterExemptionDetail].MeterExemptionId to [Information].[MeterExemption].MeterExemptionId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterExemptionDetail', N'CONSTRAINT', N'FK_MeterExemptionDetail_MeterExemptionId'
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	FK_MeterExemptionDetail_MeterExemptionAttributeId FOREIGN KEY
	(
	MeterExemptionAttributeId
	) REFERENCES [Information].[MeterExemptionAttribute]
	(
	MeterExemptionAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterExemptionDetail].MeterExemptionAttributeId to [Information].[MeterExemptionAttribute].MeterExemptionAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterExemptionDetail', N'CONSTRAINT', N'FK_MeterExemptionDetail_MeterExemptionAttributeId'
GO
ALTER TABLE [Information].[MeterExemptionDetail] ADD CONSTRAINT
	FK_MeterExemptionDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[MeterExemptionDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'MeterExemptionDetail', N'CONSTRAINT', N'FK_MeterExemptionDetail_SourceId'
GO
ALTER TABLE [Information].[MeterExemptionDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
