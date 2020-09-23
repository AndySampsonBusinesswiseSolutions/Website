USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByYearByForecastGroup.api')
    BEGIN
       CREATE LOGIN [MapFutureDateIdToUsageDateIdByYearByForecastGroup.api] WITH PASSWORD=N'MaUYGkJy3PLeLT4Y', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
