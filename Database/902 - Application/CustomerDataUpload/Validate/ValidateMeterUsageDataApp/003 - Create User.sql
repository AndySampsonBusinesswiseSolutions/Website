USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterUsageDataApp')
    BEGIN
        CREATE USER [ValidateMeterUsageDataApp] FOR LOGIN [ValidateMeterUsageDataApp]
    END
GO

ALTER USER [ValidateMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
