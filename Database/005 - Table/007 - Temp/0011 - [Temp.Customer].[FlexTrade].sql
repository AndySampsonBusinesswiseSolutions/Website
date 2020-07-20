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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.Customer].[FlexTrade]') AND type in (N'U'))
DROP TABLE [Temp.Customer].[FlexTrade]
GO
CREATE TABLE [Temp.Customer].[FlexTrade]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	BasketReference VARCHAR(255),
	TradeDate VARCHAR(255),
	TradeProduct VARCHAR(255),
	Volume VARCHAR(255),
	Price VARCHAR(255),
	Direction VARCHAR(255)
	)  ON [Temp]
GO
ALTER TABLE [Temp.Customer].[FlexTrade] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
