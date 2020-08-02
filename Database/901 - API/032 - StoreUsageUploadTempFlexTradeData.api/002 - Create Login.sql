USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFlexTradeData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempFlexTradeData.api] WITH PASSWORD=N'A5BYZuEtTQE5TENu', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
