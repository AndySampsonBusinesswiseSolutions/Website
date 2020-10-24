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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[TradeToTradeProduct]') AND type in (N'U'))
DROP TABLE [Mapping].[TradeToTradeProduct]
GO
CREATE TABLE [Mapping].[TradeToTradeProduct]
	(
	TradeToTradeProductId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TradeId BIGINT NOT NULL,
	TradeProductId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	PK_TradeToTradeProduct PRIMARY KEY CLUSTERED 
	(
	TradeToTradeProductId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	DF_TradeToTradeProduct_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	DF_TradeToTradeProduct_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	DF_TradeToTradeProduct_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	FK_TradeToTradeProduct_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeProduct].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeProduct', N'CONSTRAINT', N'FK_TradeToTradeProduct_CreatedByUserId'
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	FK_TradeToTradeProduct_TradeProductId FOREIGN KEY
	(
	TradeProductId
	) REFERENCES [Information].[TradeProduct]
	(
	TradeProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeProduct].TradeProductId to [Information].[TradeProduct].TradeProductId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeProduct', N'CONSTRAINT', N'FK_TradeToTradeProduct_TradeProductId'
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	FK_TradeToTradeProduct_TradeId FOREIGN KEY
	(
	TradeId
	) REFERENCES [Customer].[Trade]
	(
	TradeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeProduct].TradeId to [Customer].[Trade].TradeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeProduct', N'CONSTRAINT', N'FK_TradeToTradeProduct_TradeId'
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] ADD CONSTRAINT
	FK_TradeToTradeProduct_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeProduct].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeProduct', N'CONSTRAINT', N'FK_TradeToTradeProduct_SourceId'
GO
ALTER TABLE [Mapping].[TradeToTradeProduct] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
