USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByDate.api')
    BEGIN
       CREATE LOGIN [MapFutureDateIdToUsageDateIdByDate.api] WITH PASSWORD=N'jNDWrGha9gGs4Z5B', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
