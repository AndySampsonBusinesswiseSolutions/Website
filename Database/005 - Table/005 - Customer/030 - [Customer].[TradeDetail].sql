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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[TradeDetail]') AND type in (N'U'))
DROP TABLE [Customer].[TradeDetail]
GO
CREATE TABLE [Customer].[TradeDetail]
	(
	TradeDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	TradeId BIGINT NOT NULL,
	TradeAttributeId BIGINT NOT NULL,
	TradeDetailDescription VARCHAR(255) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	PK_TradeDetail PRIMARY KEY CLUSTERED 
	(
	TradeDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	DF_TradeDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	DF_TradeDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	DF_TradeDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	FK_TradeDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[TradeDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'TradeDetail', N'CONSTRAINT', N'FK_TradeDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	FK_TradeDetail_TradeId FOREIGN KEY
	(
	TradeId
	) REFERENCES [Customer].[Trade]
	(
	TradeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[TradeDetail].TradeId to [Customer].[Trade].TradeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'TradeDetail', N'CONSTRAINT', N'FK_TradeDetail_TradeId'
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	FK_TradeDetail_TradeAttributeId FOREIGN KEY
	(
	TradeAttributeId
	) REFERENCES [Customer].[TradeAttribute]
	(
	TradeAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[TradeDetail].TradeAttributeId to [Customer].[TradeAttribute].TradeAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'TradeDetail', N'CONSTRAINT', N'FK_TradeDetail_TradeAttributeId'
GO
ALTER TABLE [Customer].[TradeDetail] ADD CONSTRAINT
	FK_TradeDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[TradeDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'TradeDetail', N'CONSTRAINT', N'FK_TradeDetail_SourceId'
GO
ALTER TABLE [Customer].[TradeDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
