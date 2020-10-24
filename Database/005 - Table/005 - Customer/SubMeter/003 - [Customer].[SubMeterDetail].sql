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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[SubMeterDetail]') AND type in (N'U'))
DROP TABLE [Customer].[SubMeterDetail]
GO
CREATE TABLE [Customer].[SubMeterDetail]
	(
	SubMeterDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	SubMeterId BIGINT NOT NULL,
	SubMeterAttributeId BIGINT NOT NULL,
	SubMeterDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	PK_SubMeterDetail PRIMARY KEY CLUSTERED 
	(
	SubMeterDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	DF_SubMeterDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	DF_SubMeterDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	DF_SubMeterDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	FK_SubMeterDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[SubMeterDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'SubMeterDetail', N'CONSTRAINT', N'FK_SubMeterDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	FK_SubMeterDetail_SubMeterId FOREIGN KEY
	(
	SubMeterId
	) REFERENCES [Customer].[SubMeter]
	(
	SubMeterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[SubMeterDetail].SubMeterId to [Customer].[SubMeter].SubMeterId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'SubMeterDetail', N'CONSTRAINT', N'FK_SubMeterDetail_SubMeterId'
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	FK_SubMeterDetail_SubMeterAttributeId FOREIGN KEY
	(
	SubMeterAttributeId
	) REFERENCES [Customer].[SubMeterAttribute]
	(
	SubMeterAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[SubMeterDetail].SubMeterAttributeId to [Customer].[SubMeterAttribute].SubMeterAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'SubMeterDetail', N'CONSTRAINT', N'FK_SubMeterDetail_SubMeterAttributeId'
GO
ALTER TABLE [Customer].[SubMeterDetail] ADD CONSTRAINT
	FK_SubMeterDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[SubMeterDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'SubMeterDetail', N'CONSTRAINT', N'FK_SubMeterDetail_SourceId'
GO
ALTER TABLE [Customer].[SubMeterDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
