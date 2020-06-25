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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[MeterAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[MeterAttribute]
GO
CREATE TABLE [Customer].[MeterAttribute]
	(
	MeterAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	MeterAttributeDescription VARCHAR(200) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	PK_MeterAttribute PRIMARY KEY CLUSTERED 
	(
	MeterAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	DF_MeterAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	DF_MeterAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	DF_MeterAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	FK_MeterAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterAttribute', N'CONSTRAINT', N'FK_MeterAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[MeterAttribute] ADD CONSTRAINT
	FK_MeterAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[MeterAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'MeterAttribute', N'CONSTRAINT', N'FK_MeterAttribute_SourceId'
GO
ALTER TABLE [Customer].[MeterAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
