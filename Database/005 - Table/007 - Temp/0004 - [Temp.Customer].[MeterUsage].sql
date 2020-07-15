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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.Customer].[MeterUsage]') AND type in (N'U'))
DROP TABLE [Temp.Customer].[MeterUsage]
GO
CREATE TABLE [Temp.Customer].[MeterUsage]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	MPXN VARCHAR(255),
	Date VARCHAR(255),
	TimePeriod VARCHAR(255),
	Value VARCHAR(255)
	)  ON [Temp]
GO
ALTER TABLE [Temp.Customer].[MeterUsage] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
