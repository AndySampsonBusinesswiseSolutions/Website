USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapFutureDateIdToUsageDateIdByYearByForecastGroup.api')
    BEGIN
        CREATE USER [MapFutureDateIdToUsageDateIdByYearByForecastGroup.api] FOR LOGIN [MapFutureDateIdToUsageDateIdByYearByForecastGroup.api]
    END
GO

ALTER USER [MapFutureDateIdToUsageDateIdByYearByForecastGroup.api] WITH DEFAULT_SCHEMA=[System]
GO
