USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSubMeterUsageDataApp')
    BEGIN
        CREATE USER [ValidateSubMeterUsageDataApp] FOR LOGIN [ValidateSubMeterUsageDataApp]
    END
GO

ALTER USER [ValidateSubMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
