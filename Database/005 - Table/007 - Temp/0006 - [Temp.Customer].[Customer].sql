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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp.Customer].[Customer]') AND type in (N'U'))
DROP TABLE [Temp.Customer].[Customer]
GO
CREATE TABLE [Temp.Customer].[Customer]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER,
	CustomerName VARCHAR(255),
	ContactName VARCHAR(255),
	ContactTelephoneNumber VARCHAR(255),
	ContactEmailAddress VARCHAR(255)
	)  ON [Temp]
GO
ALTER TABLE [Temp.Customer].[Customer] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
