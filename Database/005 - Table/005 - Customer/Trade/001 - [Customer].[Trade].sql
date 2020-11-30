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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[Trade]') AND type in (N'U'))
DROP TABLE [Customer].[Trade]
GO
CREATE TABLE [Customer].[Trade]
	(
	TradeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TradeGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [Customer]
GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	PK_Trade PRIMARY KEY CLUSTERED 
	(
	TradeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	DF_Trade_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	DF_Trade_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	DF_Trade_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	FK_Trade_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[Trade].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'Trade', N'CONSTRAINT', N'FK_Trade_CreatedByUserId'
GO
ALTER TABLE [Customer].[Trade] ADD CONSTRAINT
	FK_Trade_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[Trade].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'Trade', N'CONSTRAINT', N'FK_Trade_SourceId'
GO
ALTER TABLE [Customer].[Trade] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
