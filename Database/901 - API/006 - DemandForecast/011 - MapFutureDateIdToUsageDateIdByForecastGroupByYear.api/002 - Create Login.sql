USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByForecastGroupByYear.api')
    BEGIN
       CREATE LOGIN [MapFutureDateIdToUsageDateIdByForecastGroupByYear.api] WITH PASSWORD=N'EnZrbJgUy5dxZB4T', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
