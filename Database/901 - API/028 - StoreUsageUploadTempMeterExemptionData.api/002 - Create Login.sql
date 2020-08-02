USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterExemptionData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempMeterExemptionData.api] WITH PASSWORD=N'CNs2z2TrsqzZMu2J', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
