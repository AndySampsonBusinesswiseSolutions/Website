USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateId.api')
    BEGIN
       CREATE LOGIN [MapFutureDateIdToUsageDateId.api] WITH PASSWORD=N'x3pnwJjw9YfgEtFr', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
