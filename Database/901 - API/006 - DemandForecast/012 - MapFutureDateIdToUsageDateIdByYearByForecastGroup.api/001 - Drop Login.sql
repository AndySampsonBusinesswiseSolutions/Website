USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByYearByForecastGroup.api')
    BEGIN
        DROP LOGIN [MapFutureDateIdToUsageDateIdByYearByForecastGroup.api]
    END
GO
