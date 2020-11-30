USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'DetermineFileTypeApp')
    BEGIN
        CREATE USER [DetermineFileTypeApp] FOR LOGIN [DetermineFileTypeApp]
    END
GO

ALTER USER [DetermineFileTypeApp] WITH DEFAULT_SCHEMA=[System]
GO
