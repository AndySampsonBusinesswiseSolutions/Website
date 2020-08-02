USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempSubMeterData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempSubMeterData.api] WITH PASSWORD=N'HHq85F87Ymc7P4X7', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
