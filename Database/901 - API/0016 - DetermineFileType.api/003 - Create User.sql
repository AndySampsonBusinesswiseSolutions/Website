USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'DetermineFileType.api')
    BEGIN
        CREATE USER [DetermineFileType.api] FOR LOGIN [DetermineFileType.api]
    END
GO

ALTER USER [DetermineFileType.api] WITH DEFAULT_SCHEMA=[System]
GO
