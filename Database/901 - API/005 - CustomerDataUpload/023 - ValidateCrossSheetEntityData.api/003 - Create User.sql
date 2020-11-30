USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateCrossSheetEntityData.api')
    BEGIN
        CREATE USER [ValidateCrossSheetEntityData.api] FOR LOGIN [ValidateCrossSheetEntityData.api]
    END
GO

ALTER USER [ValidateCrossSheetEntityData.api] WITH DEFAULT_SCHEMA=[System]
GO