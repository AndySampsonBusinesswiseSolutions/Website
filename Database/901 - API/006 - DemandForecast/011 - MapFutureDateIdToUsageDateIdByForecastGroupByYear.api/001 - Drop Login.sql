USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByForecastGroupByYear.api')
    BEGIN
        DROP LOGIN [MapFutureDateIdToUsageDateIdByForecastGroupByYear.api]
    END
GO
