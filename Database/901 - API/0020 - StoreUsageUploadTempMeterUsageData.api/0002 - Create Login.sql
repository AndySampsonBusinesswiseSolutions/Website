USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterUsageData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempMeterUsageData.api] WITH PASSWORD=N'Xrx7E74XsVQeMqy7', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
