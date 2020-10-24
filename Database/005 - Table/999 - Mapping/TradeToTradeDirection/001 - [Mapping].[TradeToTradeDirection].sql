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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[TradeToTradeDirection]') AND type in (N'U'))
DROP TABLE [Mapping].[TradeToTradeDirection]
GO
CREATE TABLE [Mapping].[TradeToTradeDirection]
	(
	TradeToTradeDirectionId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TradeId BIGINT NOT NULL,
	TradeDirectionId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	PK_TradeToTradeDirection PRIMARY KEY CLUSTERED 
	(
	TradeToTradeDirectionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	DF_TradeToTradeDirection_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	DF_TradeToTradeDirection_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	DF_TradeToTradeDirection_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	FK_TradeToTradeDirection_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeDirection].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeDirection', N'CONSTRAINT', N'FK_TradeToTradeDirection_CreatedByUserId'
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	FK_TradeToTradeDirection_TradeDirectionId FOREIGN KEY
	(
	TradeDirectionId
	) REFERENCES [Information].[TradeDirection]
	(
	TradeDirectionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeDirection].TradeDirectionId to [Information].[TradeDirection].TradeDirectionId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeDirection', N'CONSTRAINT', N'FK_TradeToTradeDirection_TradeDirectionId'
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	FK_TradeToTradeDirection_TradeId FOREIGN KEY
	(
	TradeId
	) REFERENCES [Customer].[Trade]
	(
	TradeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeDirection].TradeId to [Customer].[Trade].TradeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeDirection', N'CONSTRAINT', N'FK_TradeToTradeDirection_TradeId'
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] ADD CONSTRAINT
	FK_TradeToTradeDirection_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[TradeToTradeDirection].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'TradeToTradeDirection', N'CONSTRAINT', N'FK_TradeToTradeDirection_SourceId'
GO
ALTER TABLE [Mapping].[TradeToTradeDirection] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
