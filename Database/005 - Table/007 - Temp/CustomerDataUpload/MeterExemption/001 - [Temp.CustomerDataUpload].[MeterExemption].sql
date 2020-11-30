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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[MeterExemption]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[MeterExemption]
GO
CREATE TABLE [Temp.CustomerDataUpload].[MeterExemption]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	MPXN VARCHAR(255),
	DateFrom VARCHAR(255),
	DateTo VARCHAR(255),
	ExemptionProduct VARCHAR(255),
	ExemptionProportion VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[MeterExemption] ADD CONSTRAINT
	DF_MeterExemption_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[MeterExemption] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
