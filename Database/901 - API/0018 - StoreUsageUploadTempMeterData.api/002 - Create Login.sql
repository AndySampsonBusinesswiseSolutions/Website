USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempMeterData.api] WITH PASSWORD=N'EqsVJUK59sxf8QsE', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
