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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[BasketAttribute]') AND type in (N'U'))
DROP TABLE [Customer].[BasketAttribute]
GO
CREATE TABLE [Customer].[BasketAttribute]
	(
	BasketAttributeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	BasketAttributeDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	PK_BasketAttribute PRIMARY KEY CLUSTERED 
	(
	BasketAttributeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	DF_BasketAttribute_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	DF_BasketAttribute_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	DF_BasketAttribute_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	FK_BasketAttribute_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketAttribute].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketAttribute', N'CONSTRAINT', N'FK_BasketAttribute_CreatedByUserId'
GO
ALTER TABLE [Customer].[BasketAttribute] ADD CONSTRAINT
	FK_BasketAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketAttribute', N'CONSTRAINT', N'FK_BasketAttribute_SourceId'
GO
ALTER TABLE [Customer].[BasketAttribute] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
