USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterUsageDataApp')
    BEGIN
       CREATE LOGIN [StoreSubMeterUsageDataApp] WITH PASSWORD=N'uKxeuMwrdks8nXSa', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
