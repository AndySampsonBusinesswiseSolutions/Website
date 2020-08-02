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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[BasketDetail]') AND type in (N'U'))
DROP TABLE [Customer].[BasketDetail]
GO
CREATE TABLE [Customer].[BasketDetail]
	(
	BasketDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	BasketId BIGINT NOT NULL,
	BasketAttributeId BIGINT NOT NULL,
	BasketDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	PK_BasketDetail PRIMARY KEY CLUSTERED 
	(
	BasketDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	DF_BasketDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	DF_BasketDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	DF_BasketDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	FK_BasketDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketDetail', N'CONSTRAINT', N'FK_BasketDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	FK_BasketDetail_BasketId FOREIGN KEY
	(
	BasketId
	) REFERENCES [Customer].[Basket]
	(
	BasketId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketDetail].BasketId to [Customer].[Basket].BasketId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketDetail', N'CONSTRAINT', N'FK_BasketDetail_BasketId'
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	FK_BasketDetail_BasketAttributeId FOREIGN KEY
	(
	BasketAttributeId
	) REFERENCES [Customer].[BasketAttribute]
	(
	BasketAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketDetail].BasketAttributeId to [Customer].[BasketAttribute].BasketAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketDetail', N'CONSTRAINT', N'FK_BasketDetail_BasketAttributeId'
GO
ALTER TABLE [Customer].[BasketDetail] ADD CONSTRAINT
	FK_BasketDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[BasketDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'BasketDetail', N'CONSTRAINT', N'FK_BasketDetail_SourceId'
GO
ALTER TABLE [Customer].[BasketDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
