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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.Customer].[Meter]') AND type in (N'U'))
DROP TABLE [Temp.Customer].[Meter]
GO
CREATE TABLE [Temp.Customer].[Meter]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	Site VARCHAR(255),
	MPXN VARCHAR(255),
	ProfileClass VARCHAR(255),
	MeterTimeswitchClass VARCHAR(255),
	LineLossFactorClass VARCHAR(255),
	Capacity VARCHAR(255),
	LocalDistributionZone VARCHAR(255),
	StandardOfftakeQuantity VARCHAR(255),
	AnnualUsage VARCHAR(255),
	DayUsage VARCHAR(255),
	NightUsage VARCHAR(255)
	)  ON [Temp]
GO
ALTER TABLE [Temp.Customer].[Meter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
