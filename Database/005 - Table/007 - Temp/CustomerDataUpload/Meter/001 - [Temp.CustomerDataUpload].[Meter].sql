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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[Meter]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[Meter]
GO
CREATE TABLE [Temp.CustomerDataUpload].[Meter]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	SiteName VARCHAR(255),
	SitePostCode VARCHAR(255),
	MPXN VARCHAR(255),
	GridSupplyPoint VARCHAR(255),
	ProfileClass VARCHAR(255),
	MeterTimeswitchCode VARCHAR(255),
	LineLossFactorClass VARCHAR(255),
	Capacity VARCHAR(255),
	LocalDistributionZone VARCHAR(255),
	StandardOfftakeQuantity VARCHAR(255),
	AnnualUsage VARCHAR(255),
	MeterSerialNumber VARCHAR(255),
	Area VARCHAR(255),
	ImportExport VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[Meter] ADD CONSTRAINT
	DF_Meter_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[Meter] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
