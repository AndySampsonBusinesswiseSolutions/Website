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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[GridSupplyPointDetail]') AND type in (N'U'))
DROP TABLE [Information].[GridSupplyPointDetail]
GO
CREATE TABLE [Information].[GridSupplyPointDetail]
	(
	GridSupplyPointDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	GridSupplyPointId BIGINT NOT NULL,
	GridSupplyPointAttributeId BIGINT NOT NULL,
	GridSupplyPointDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	PK_GridSupplyPointDetail PRIMARY KEY CLUSTERED 
	(
	GridSupplyPointDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	DF_GridSupplyPointDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	DF_GridSupplyPointDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	DF_GridSupplyPointDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	FK_GridSupplyPointDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointDetail', N'CONSTRAINT', N'FK_GridSupplyPointDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	FK_GridSupplyPointDetail_GridSupplyPointId FOREIGN KEY
	(
	GridSupplyPointId
	) REFERENCES [Information].[GridSupplyPoint]
	(
	GridSupplyPointId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointDetail].GridSupplyPointId to [Information].[GridSupplyPoint].GridSupplyPointId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointDetail', N'CONSTRAINT', N'FK_GridSupplyPointDetail_GridSupplyPointId'
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	FK_GridSupplyPointDetail_GridSupplyPointAttributeId FOREIGN KEY
	(
	GridSupplyPointAttributeId
	) REFERENCES [Information].[GridSupplyPointAttribute]
	(
	GridSupplyPointAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointDetail].GridSupplyPointAttributeId to [Information].[GridSupplyPointAttribute].GridSupplyPointAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointDetail', N'CONSTRAINT', N'FK_GridSupplyPointDetail_GridSupplyPointAttributeId'
GO
ALTER TABLE [Information].[GridSupplyPointDetail] ADD CONSTRAINT
	FK_GridSupplyPointDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[GridSupplyPointDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'GridSupplyPointDetail', N'CONSTRAINT', N'FK_GridSupplyPointDetail_SourceId'
GO
ALTER TABLE [Information].[GridSupplyPointDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
