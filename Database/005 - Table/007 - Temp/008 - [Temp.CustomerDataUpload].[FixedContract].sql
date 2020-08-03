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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[FixedContract]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[FixedContract]
GO
CREATE TABLE [Temp.CustomerDataUpload].[FixedContract]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	ContractReference VARCHAR(255),
	MPXN VARCHAR(255),
	Supplier VARCHAR(255),
	ContractStartDate VARCHAR(255),
	ContractEndDate VARCHAR(255),
	Product VARCHAR(255),
	RateCount VARCHAR(255),
	StandingCharge VARCHAR(255),
	CapacityCharge VARCHAR(255),
	Rate VARCHAR(255),
	Value VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[FixedContract] ADD CONSTRAINT
	DF_Site_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[FixedContract] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
