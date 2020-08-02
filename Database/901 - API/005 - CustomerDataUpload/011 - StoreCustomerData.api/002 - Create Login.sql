USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreCustomerData.api')
    BEGIN
       CREATE LOGIN [StoreCustomerData.api] WITH PASSWORD=N'qkaux33qraa6EZ9H', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
