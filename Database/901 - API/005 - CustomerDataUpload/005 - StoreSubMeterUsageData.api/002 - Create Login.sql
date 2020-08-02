USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterUsageData.api')
    BEGIN
       CREATE LOGIN [StoreSubMeterUsageData.api] WITH PASSWORD=N'uKxeuMwrdks8nXSa', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
