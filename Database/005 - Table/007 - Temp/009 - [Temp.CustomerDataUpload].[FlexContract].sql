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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[FlexContract]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[FlexContract]
GO
CREATE TABLE [Temp.CustomerDataUpload].[FlexContract]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	ContractReference VARCHAR(255),
	BasketReference VARCHAR(255),
	MPXN VARCHAR(255),
	Supplier VARCHAR(255),
	ContractStartDate VARCHAR(255),
	ContractEndDate VARCHAR(255),
	Product VARCHAR(255),
	StandingCharge VARCHAR(255),	
	RateType VARCHAR(255),
	Value VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[FlexContract] ADD CONSTRAINT
	DF_FlexContract_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[FlexContract] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
