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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[FlexReferenceVolume]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[FlexReferenceVolume]
GO
CREATE TABLE [Temp.CustomerDataUpload].[FlexReferenceVolume]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	ContractReference VARCHAR(255),
	DateFrom VARCHAR(255),
	DateTo VARCHAR(255),
	Volume VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[FlexReferenceVolume] ADD CONSTRAINT
	DF_FlexReferenceVolume_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[FlexReferenceVolume] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
