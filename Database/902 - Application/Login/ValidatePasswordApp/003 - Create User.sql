USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePasswordApp')
    BEGIN
        CREATE USER [ValidatePasswordApp] FOR LOGIN [ValidatePasswordApp]
    END
GO

ALTER USER [ValidatePasswordApp] WITH DEFAULT_SCHEMA=[System]
GO