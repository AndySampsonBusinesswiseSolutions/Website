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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.CustomerDataUpload].[Customer]') AND type in (N'U'))
DROP TABLE [Temp.CustomerDataUpload].[Customer]
GO
CREATE TABLE [Temp.CustomerDataUpload].[Customer]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	RowId INT,
	CustomerName VARCHAR(255),
	ContactName VARCHAR(255),
	ContactTelephoneNumber VARCHAR(255),
	ContactEmailAddress VARCHAR(255),
	CanCommit BIT
	)  ON [Temp]
GO
ALTER TABLE [Temp.CustomerDataUpload].[Customer] ADD CONSTRAINT
	DF_Customer_CanCommit DEFAULT 0 FOR CanCommit
GO
ALTER TABLE [Temp.CustomerDataUpload].[Customer] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
