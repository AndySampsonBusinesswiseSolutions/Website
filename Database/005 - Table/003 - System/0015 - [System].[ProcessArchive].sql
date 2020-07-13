USE [EMaaS]
GO

IF EXISTS(SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProcessArchive_HasError')
	BEGIN
  		ALTER TABLE [System].[ProcessArchive] DROP CONSTRAINT DF_ProcessArchive_HasError;	
	END

IF COL_LENGTH('[System].[ProcessArchive]', 'HasError') IS NOT NULL
	BEGIN
		ALTER TABLE [System].[ProcessArchive] DROP COLUMN HasError
	END
GO

ALTER TABLE [System].[ProcessArchive] ADD HasError BIT NULL
GO

UPDATE
	[System].[ProcessArchive]
SET
	HasError = 0

ALTER TABLE [System].[ProcessArchive] ADD CONSTRAINT
	DF_ProcessArchive_HasError DEFAULT 0 FOR HasError
GO

ALTER TABLE [System].[ProcessArchive] ALTER COLUMN HasError BIT NOT NULL
GO
