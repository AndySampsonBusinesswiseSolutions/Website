USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomerApp')
    BEGIN
        CREATE LOGIN [MapCustomerToChildCustomerApp] WITH PASSWORD=N'6dFB@tk?7L$UrQ9p', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO